using System.Linq.Expressions;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IURLsViewingService
    {
        Task<URLsViewingResult> ViewURLsAsync(int creatorId);
    }

    public class URLsViewingResult
    {
        public List<URL>? URLs { get; private set; }

        public URLsOperationResultCode Status { get; private set; }

        public URLsViewingResult(List<URL>? urls, URLsOperationResultCode status)
        {
            URLs = urls;
            Status = status;
        }
    }
}
