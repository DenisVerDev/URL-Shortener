import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  computed,
  inject,
  OnInit,
  signal
} from '@angular/core';
import { finalize } from 'rxjs';

import { UrlDto } from './urls-table.models';
import { UrlsTableService } from './urls-table.service';

@Component({
  selector: 'app-urls-table',
  standalone: true,
  templateUrl: './urls-table.component.html',
  styleUrl: './urls-table.component.css'
})
export class UrlsTableComponent implements OnInit {
  private readonly urlsService =
    inject(UrlsTableService);

  protected readonly urls = signal<UrlDto[]>([]);
  protected readonly pageIndex = signal(0);
  protected readonly totalCount = signal(0);
  protected readonly totalPages = signal(0);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal('');

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
          this.handleError(error);
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

  private handleError(error: HttpErrorResponse): void {
    if (error.status === 400) {
      this.errorMessage.set(
        'The pagination parameters are invalid.');

      return;
    }

    this.errorMessage.set(
      'The URLs could not be loaded. Please try again.');
  }
}
