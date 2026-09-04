export interface UrlDto {
  id: number;
  isUserAuthority: boolean;
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

export enum URLsOperationResultCode {
  Success = 0,
  DuplicateURL = 1,
  AbsentURL = 2,
  AbsentURLs = 3,
  AbsentUser = 4
}
