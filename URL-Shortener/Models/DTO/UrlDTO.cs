namespace URL_Shortener.Models.DTO
{
    public class UrlDTO
    {
        public int Id {  get; set; }

        public int CreatorId { get; set; } // will come in hand later

        public string OriginalURL { get; set; } = null!;

        public string ShortURLId { get; set; } = null!;
    }
}
