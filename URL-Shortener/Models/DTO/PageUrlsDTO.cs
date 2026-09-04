namespace URL_Shortener.Models.DTO
{
    public class PageUrlsDTO
    {
        public List<UrlDTO> Items { get; set; } = [];

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
