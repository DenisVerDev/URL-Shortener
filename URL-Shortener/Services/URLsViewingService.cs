using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using URL_Shortener.Data.Models;
using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services
{
    public class URLsViewingService(IUsersRepository _ur, IURLsRepository _urlsR) : IURLsViewingService
    {
        public virtual async Task<URLViewingResult> ViewURLAsync(int id)
        {
            var url = await _urlsR.FindURLAsync(id);
            var status = url is null ? URLsOperationResultCode.AbsentURL : URLsOperationResultCode.Success;

            return new URLViewingResult(url, status);
        }

        public virtual async Task<URLViewingResult> ViewURLAsync(string originalURL)
        {
            if (originalURL.IsNullOrEmpty())
                throw new ArgumentException($"{nameof(URLsViewingService)} cannot view URL entity by null or empty {nameof(originalURL)}!");

            var url = await _urlsR.FindURLAsync(originalURL);
            var status = url is null ? URLsOperationResultCode.AbsentURL : URLsOperationResultCode.Success;

            return new URLViewingResult(url, status);
        }

        public virtual async Task<URLViewingResult> ViewShortURLAsync(string shortUrlId)
        {
            if (shortUrlId.IsNullOrEmpty())
                throw new ArgumentException($"{nameof(URLsViewingService)} cannot view URL entity by null or empty {nameof(shortUrlId)}!");

            var url = await _urlsR.FirstURLAsync(u => u.ShortURLId == shortUrlId);
            var status = url is null ? URLsOperationResultCode.AbsentURL : URLsOperationResultCode.Success;

            return new URLViewingResult(url, status);
        }

        public virtual async Task<URLsViewingResult> ViewURLsAsync(int creatorId)
        {
            if (!await _ur.AnyUserAsync(u => u.Id == creatorId))
                return new URLsViewingResult(null, URLsOperationResultCode.AbsentUser);

            var urls = await _urlsR.FindURLsAsync(creatorId);

            return new URLsViewingResult(urls, URLsOperationResultCode.Success);
        }

        public async Task<URLsViewingResult> ViewURLsAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
                throw new ArgumentException($"{nameof(URLsViewingService)} cannot view URLs when {nameof(pageIndex)} or {nameof(pageSize)} is lesser than zero!");

            var urls = await _urlsR.FindURLsAsync(pageIndex, pageSize);
            return new URLsViewingResult(urls, URLsOperationResultCode.Success);
        }

        public async Task<int> ViewURLsCountAsync()
            => await _urlsR.CountURLsAsync();
    }
}
