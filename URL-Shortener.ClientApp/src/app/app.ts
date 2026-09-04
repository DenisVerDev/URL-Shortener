import { Component } from '@angular/core';

import {
  UrlShortenerComponent
} from './url-shortener/url-shortener.component';

@Component({
  selector: 'app-root',
  imports: [
    UrlShortenerComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
}
