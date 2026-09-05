using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IURLsManagementService
    {
        Task<URLCreationResult> CreateURLAsync(string originalUrl, int creatorId);

        Task<URLsOperationResultCode> DeleteURLAsync(URL url);

        Task<URLsOperationResultCode> DeleteURLsAsync(int creatorId);
    }

    public class URLCreationResult
    {
        public URL? URL { get; private set; }

        public URLsOperationResultCode Status { get; private set; }

        public URLCreationResult(URL? url, URLsOperationResultCode status)
        {
            URL = url;
            Status = status;
        }
    }

    public enum URLsOperationResultCode
    {
        Success,
        DuplicateURL,
        AbsentURL,
        AbsentURLs,
        AbsentUser
    }
}
