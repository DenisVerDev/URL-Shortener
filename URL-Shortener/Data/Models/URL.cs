using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Data.Models
{
    public class URL
    {
        public int Id { get; set; }

        public string OriginalURL { get; set; } = null!;

        public string ShortURLId { get; set; } = null!;

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public int CreatorId { get; set; }

        public User Creator { get; set; } = null!;
    }
}
