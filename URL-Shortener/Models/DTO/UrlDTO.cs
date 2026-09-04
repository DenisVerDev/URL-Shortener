namespace URL_Shortener.Models.DTO
{
    public class UrlDTO
    {
        public int Id {  get; set; }

        public bool IsUserCreator { get; set; } // meaning current user who did the request

        public string OriginalURL { get; set; } = null!;

        public string ShortURLId { get; set; } = null!;
    }
}
