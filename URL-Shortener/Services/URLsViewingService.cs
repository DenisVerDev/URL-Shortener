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
            var url = await _urlsR.FindURLAsync(originalURL);
            var status = url is null ? URLsOperationResultCode.AbsentURL : URLsOperationResultCode.Success;

            return new URLViewingResult(url, status);
        }

        public virtual async Task<URLViewingResult> ViewShortURLAsync(string shortUrlId)
        {
            var url = await _urlsR.FirstURLAsync(u=>u.ShortURLId == shortUrlId);
            var status = url is null ? URLsOperationResultCode.AbsentURL : URLsOperationResultCode.Success;

            return new URLViewingResult(url, status);
        }

        public virtual async Task<URLsViewingResult> ViewURLsAsync(int creatorId)
        {
            if(!await _ur.AnyUserAsync(u=>u.Id == creatorId))
                return new URLsViewingResult(null, URLsOperationResultCode.AbsentUser);

            var urls = await _urlsR.FindURLsAsync(creatorId);

            return new URLsViewingResult(urls, URLsOperationResultCode.Success);
        }

        public async Task<URLsViewingResult> ViewURLsAsync(int pageIndex, int pageSize)
        {
            var urls = await _urlsR.FindURLsAsync(pageIndex, pageSize);
            return new URLsViewingResult(urls, URLsOperationResultCode.Success);
        }

        public async Task<int> ViewURLsCountAsync()
            => await _urlsR.CountURLsAsync();
    }
}
