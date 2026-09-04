import {
  Component,
  ElementRef,
  inject,
  viewChild
} from '@angular/core';

import {
  UrlShortenerComponent
} from './url-shortener/url-shortener.component';

import {
  UrlsTableComponent
} from './urls-table/urls-table.component';

@Component({
  selector: 'app-root',
  imports: [
    UrlShortenerComponent,
    UrlsTableComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private readonly hostElement =
    inject(ElementRef<HTMLElement>);

  private readonly urlsTable =
    viewChild(UrlsTableComponent);

  protected readonly isAuthenticated =
    this.hostElement.nativeElement
      .dataset['isAuthenticated'] !== 'false';

  protected handleUrlShortened(): void {
    this.urlsTable()?.showFirstPage();
  }
}
