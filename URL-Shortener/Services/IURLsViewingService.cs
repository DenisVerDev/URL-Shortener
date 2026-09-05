using System.Linq.Expressions;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IURLsViewingService
    {
        Task<URLViewingResult> ViewURLAsync(int id);

        Task<URLViewingResult> ViewURLAsync(string originalURL);

        Task<URLViewingResult> ViewShortURLAsync(string shortUrlId);

        Task<URLsViewingResult> ViewURLsAsync(int creatorId);

        Task<URLsViewingResult> ViewURLsAsync(int pageIndex = 0, int pageSize = 10);

        Task<int> ViewURLsCountAsync();
    }

    public class URLViewingResult
    {
        public URL? URL { get; private set; }

        public URLsOperationResultCode Status { get; private set; }

        public URLViewingResult(URL? url, URLsOperationResultCode status)
        {
            URL = url;
            Status = status;
        }
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
