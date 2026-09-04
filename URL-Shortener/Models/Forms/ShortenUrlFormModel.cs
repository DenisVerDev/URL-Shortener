using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models.Forms
{
    public class ShortenUrlFormModel
    {
        [Required(ErrorMessage = "URL was not provided.")]
        [Url]
        public string URL { get; set; } = null!;
    }
}
