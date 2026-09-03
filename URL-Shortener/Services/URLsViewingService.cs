using System.Linq.Expressions;
using URL_Shortener.Data.Models;
using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services
{
    public class URLsViewingService(IUsersRepository _ur, IURLsRepository _urlsR) : IURLsViewingService
    {
        public virtual async Task<URL?> ViewURLAsync(int id)
            => await _urlsR.FindURLAsync(id);

        public virtual async Task<URL?> ViewURLAsync(string shortUrlId)
            => await _urlsR.FirstURLAsync(u => u.ShortURLId == shortUrlId);

        public virtual async Task<URLsViewingResult> ViewURLsAsync(int creatorId)
        {
            if(!await _ur.AnyUserAsync(u=>u.Id == creatorId))
                return new URLsViewingResult(null, URLsOperationResultCode.AbsentUser);

            var urls = await _urlsR.FindURLsAsync(creatorId);

            return new URLsViewingResult(urls, URLsOperationResultCode.Success);
        }

        public virtual async Task<List<URL>> ViewURLsAsync(Expression<Func<URL, bool>> predicate)
            => await _urlsR.FindURLsAsync(predicate);
    }
}
