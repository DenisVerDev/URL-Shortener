export interface UrlDto {
  id: number;
  originalURL: string;
  shortURLId: string;
}

export interface PagedUrlsDto {
  items: UrlDto[];
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
