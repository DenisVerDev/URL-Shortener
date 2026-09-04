import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { finalize } from 'rxjs';

import { UrlShortenerService } from './url-shortener.service';

@Component({
  imports: [
    ReactiveFormsModule
  ],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html'
})
export class App {
  private readonly urlService = inject(
    UrlShortenerService);

  protected readonly form = new FormGroup({
    url: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required
      ]
    })
  });

  protected readonly shortUrlId =
    signal<string | null>(null);

  protected readonly errorMessage = signal('');

  protected readonly isSubmitting = signal(false);

  protected readonly shortenedUrl = computed(() => {
    const id = this.shortUrlId();

    if (id === null)
      return null;

    return `${window.location.origin}/${id}`;
  });

  protected get urlControl(): FormControl<string> {
    return this.form.controls.url;
  }

  protected submit(): void {
    if (this.form.invalid || this.isSubmitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.shortUrlId.set(null);
    this.errorMessage.set('');
    this.isSubmitting.set(true);

    this.urlService
      .shortenUrl(this.urlControl.value)
      .pipe(
        finalize(() => this.isSubmitting.set(false))
      )
      .subscribe({
        next: returnedShortUrlId => {
          this.shortUrlId.set(
            returnedShortUrlId.trim());
        },
        error: error => {
          this.handleError(error);
        }
      });
  }

  private handleError(error: HttpErrorResponse): void {
    if (error.status === 400) {
      const responseBody = this.parseErrorBody(
        error.error);

      const validationErrors =
        responseBody?.errors;

      this.errorMessage.set(
        validationErrors?.URL?.[0] ??
        validationErrors?.url?.[0] ??
        'The supplied URL is invalid.'
      );

      return;
    }

    if (error.status === 401) {
      this.errorMessage.set(
        'You must log in before shortening URLs.');

      return;
    }

    if (error.status === 403) {
      this.errorMessage.set(
        'You do not have permission to add URLs.');

      return;
    }

    if (error.status === 409) {
      this.errorMessage.set(
        'This URL has already been shortened.');

      return;
    }

    this.errorMessage.set(
      'An unexpected error occurred.');
  }

  private parseErrorBody(body: unknown): any {
    if (typeof body !== 'string')
      return body;

    try {
      return JSON.parse(body);
    } catch {
      return null;
    }
  }
}
