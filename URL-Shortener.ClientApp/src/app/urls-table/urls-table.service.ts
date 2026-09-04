import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  PagedUrlsDto,
  URLsOperationResultCode
} from './urls-table.models';

@Injectable({
  providedIn: 'root'
})
export class UrlsTableService {
  private readonly http = inject(HttpClient);

  getUrls(
    pageIndex: number,
    pageSize: number
  ): Observable<PagedUrlsDto> {
    return this.http.get<PagedUrlsDto>(
      '/urls',
      {
        params: {
          pageIndex: pageIndex.toString(),
          pageSize: pageSize.toString()
        }
      }
    );
  }

  deleteUrl(
    id: number,
    isAdmin: boolean
  ): Observable<URLsOperationResultCode> {
    const endpoint = isAdmin
      ? `/delete/${id}`
      : `/delete/personal/${id}`;

    return this.http.delete<URLsOperationResultCode>(
      endpoint
    );
  }
}
