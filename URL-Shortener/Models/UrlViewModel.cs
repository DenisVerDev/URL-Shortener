namespace URL_Shortener.Models
{
    public class UrlViewModel
    {
        public string OriginalURL { get; set; } = null!;

        public string ShortURLId { get; set; } = null!;

        public string Creator { get; set; } = null!; // login

        public DateTime CreationDate { get; set; }
    }
}
