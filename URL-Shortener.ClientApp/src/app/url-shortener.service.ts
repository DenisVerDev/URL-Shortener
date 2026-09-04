import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  private readonly http = inject(HttpClient);

  shortenUrl(originalUrl: string): Observable<string> {
    return this.http.post(
      '/short',
      {
        URL: originalUrl
      },
      {
        responseType: 'text'
      }
    );
  }
}
