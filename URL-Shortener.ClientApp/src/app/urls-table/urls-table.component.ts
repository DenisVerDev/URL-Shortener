import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  computed,
  inject,
  input,
  OnInit,
  signal
} from '@angular/core';
import { finalize } from 'rxjs';

import {
  UrlDto,
  URLsOperationResultCode
} from './urls-table.models';

import {
  UrlsTableService
} from './urls-table.service';

@Component({
  selector: 'app-urls-table',
  standalone: true,
  templateUrl: './urls-table.component.html',
  styleUrl: './urls-table.component.css'
})
export class UrlsTableComponent implements OnInit {
  private readonly urlsService =
    inject(UrlsTableService);

  readonly isAuthenticated = input(false);
  readonly isAdmin = input(false);

  protected readonly urls = signal<UrlDto[]>([]);
  protected readonly pageIndex = signal(0);
  protected readonly totalCount = signal(0);
  protected readonly totalPages = signal(0);
  protected readonly isLoading = signal(false);

  protected readonly deletingUrlId =
    signal<number | null>(null);

  protected readonly errorMessage = signal('');
  protected readonly deletionErrorMessage = signal('');

  protected readonly pageSize = 10;

  protected readonly currentPageNumber = computed(() => {
    return this.totalPages() === 0
      ? 0
      : this.pageIndex() + 1;
  });

  protected readonly visiblePageIndexes = computed(() => {
    const totalPages = this.totalPages();

    if (totalPages === 0) {
      return [];
    }

    const maximumVisiblePages = 5;

    let start = Math.max(
      0,
      this.pageIndex() - 2);

    start = Math.min(
      start,
      Math.max(
        0,
        totalPages - maximumVisiblePages));

    const end = Math.min(
      totalPages,
      start + maximumVisiblePages);

    return Array.from(
      { length: end - start },
      (_, index) => start + index);
  });

  ngOnInit(): void {
    this.loadPage(0);
  }

  protected loadPage(pageIndex: number): void {
    if (pageIndex < 0) {
      return;
    }

    if (
      this.totalPages() > 0 &&
      pageIndex >= this.totalPages()
    ) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.urlsService
      .getUrls(pageIndex, this.pageSize)
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: response => {
          this.urls.set(response.items);
          this.pageIndex.set(response.pageIndex);
          this.totalCount.set(response.totalCount);
          this.totalPages.set(response.totalPages);
        },
        error: error => {
          this.handleLoadingError(error);
        }
      });
  }

  public showFirstPage(): void {
    this.loadPage(0);
  }

  protected previousPage(): void {
    this.loadPage(this.pageIndex() - 1);
  }

  protected nextPage(): void {
    this.loadPage(this.pageIndex() + 1);
  }

  protected shortenedUrl(shortUrlId: string): string {
    return `${window.location.origin}/short/${shortUrlId}`;
  }

  protected detailsUrl(id: number): string {
    return `/Url/Index/${id}`;
  }

  protected deleteUrl(url: UrlDto): void {
    if (!url.isUserAuthority) {
      return;
    }

    if (this.deletingUrlId() !== null) {
      return;
    }

    const confirmed = window.confirm(
      'Are you sure you want to delete this shortened URL?'
    );

    if (!confirmed) {
      return;
    }

    this.deletingUrlId.set(url.id);
    this.deletionErrorMessage.set('');

    this.urlsService
      .deleteUrl(url.id, this.isAdmin())
      .pipe(
        finalize(() => {
          this.deletingUrlId.set(null);
        })
      )
      .subscribe({
        next: result => {
          if (
            result !==
            URLsOperationResultCode.Success
          ) {
            return;
          }

          const shouldOpenPreviousPage =
            this.urls().length === 1 &&
            this.pageIndex() > 0;

          const pageToLoad =
            shouldOpenPreviousPage
              ? this.pageIndex() - 1
              : this.pageIndex();

          this.loadPage(pageToLoad);
        },
        error: error => {
          this.handleDeletionRequestError(error);
        }
      });
  }

  private handleLoadingError(
    error: HttpErrorResponse
  ): void {
    if (error.status === 400) {
      this.errorMessage.set(
        'The pagination parameters are invalid.');

      return;
    }

    this.errorMessage.set(
      'The URLs could not be loaded. Please try again.');
  }

  private handleDeletionRequestError(
    error: HttpErrorResponse
  ): void {
    if (
      error.status === 401 ||
      error.status === 403
    ) {
      this.deletionErrorMessage.set(
        'You do not have permission to delete this URL.');

      return;
    }

    this.deletionErrorMessage.set(
      'The delete request could not be completed.');
  }
}
