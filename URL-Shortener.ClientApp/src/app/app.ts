import {
  Component,
  ElementRef,
  inject
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

  /*
   * When Angular is run separately through ng serve,
   * the attribute is absent and the form remains visible.
   *
   * The MVC view always supplies either "true" or "false".
   */
  protected readonly isAuthenticated =
    this.hostElement.nativeElement
      .dataset['isAuthenticated'] !== 'false';
}
