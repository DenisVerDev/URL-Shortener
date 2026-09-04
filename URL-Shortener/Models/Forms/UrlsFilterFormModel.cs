using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models.Forms
{
    public class UrlsFilterFormModel
    {
        [Range(0, int.MaxValue)]

        public int PageIndex {  get; set; }

        [Range(1, 100)]
        public int PageSize { get; set; }
    }
}
