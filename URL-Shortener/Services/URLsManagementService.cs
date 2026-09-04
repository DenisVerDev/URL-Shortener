using Microsoft.IdentityModel.Tokens;
using URL_Shortener.Data.Models;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Services.ShorteningAlgorithms;

namespace URL_Shortener.Services
{
    public class URLsManagementService(IUsersRepository _ur, IURLsRepository _urlR, IURLShortenAlgorithm _usa) : IURLsManagementService
    {
        public async Task<URLCreationResult> CreateURLAsync(string originalUrl, int creatorId)
        {
            if (originalUrl.IsNullOrEmpty())
                throw new ArgumentException($"{nameof(URLsManagementService)} cannot create new URL with null or empty original url!");

            if(await _urlR.AnyURLAsync(u => u.OriginalURL == originalUrl))
                return new URLCreationResult(null, URLsOperationResultCode.DuplicateURL);

            if (!await _ur.AnyUserAsync(u => u.Id == creatorId))
                return new URLCreationResult(null, URLsOperationResultCode.AbsentUser);

            var shortUrlId = await _usa.ShortenURLAsync(originalUrl);
            var url = await _urlR.CreateURLAsync(originalUrl, shortUrlId!, creatorId);

            return new URLCreationResult(url, URLsOperationResultCode.Success);
        }

        public async Task<URLsOperationResultCode> DeleteURLAsync(URL url)
        {
            if (url is null)
                throw new ArgumentNullException($"{nameof(URLsManagementService)} cannot delete URL entity with null data!");

            if (!await _urlR.AnyURLAsync(u => u.Id == url.Id))
                return URLsOperationResultCode.AbsentURL;

            await _urlR.DeleteURLAsync(url);

            return URLsOperationResultCode.Success;
        }

        public async Task<URLsOperationResultCode> DeleteURLsAsync(int creatorId)
        {
            if (!await _ur.AnyUserAsync(u => u.Id == creatorId))
                return URLsOperationResultCode.AbsentUser;

            var urls = await _urlR.FindURLsAsync(creatorId);

            if (urls.IsNullOrEmpty())
                return URLsOperationResultCode.AbsentURLs;

            await _urlR.DeleteURLsAsync(urls);

            return URLsOperationResultCode.Success;
        }
    }
}
