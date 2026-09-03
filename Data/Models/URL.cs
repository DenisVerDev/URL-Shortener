using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Data.Models
{
    public class URL
    {
        [Key]
        public int Id { get; set; }

        public string OriginalURL { get; set; }

        public string ShortURLId { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public User Creator { get; set; }
    }
}
