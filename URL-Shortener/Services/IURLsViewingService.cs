using System.Linq.Expressions;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IURLsViewingService
    {
        Task<URL?> ViewURLAsync(int id);

        Task<URL?> ViewURLAsync(string shortUrlId);

        Task<URLsViewingResult> ViewURLsAsync(int creatorId);

        Task<List<URL>> ViewURLsAsync(Expression<Func<URL, bool>> predicate); // I will think about pagination later
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
