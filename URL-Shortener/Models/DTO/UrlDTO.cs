namespace URL_Shortener.Models.DTO
{
    public class UrlDTO
    {
        public int Id {  get; set; }

        public string OriginalURL { get; set; }

        public string ShortURLId { get; set; }
    }
}
