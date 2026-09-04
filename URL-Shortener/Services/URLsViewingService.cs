using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using URL_Shortener.Data.Models;
using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services
{
    public class URLsViewingService(IUsersRepository _ur, IURLsRepository _urlsR) : IURLsViewingService
    {
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

            return urls.IsNullOrEmpty() ? new URLsViewingResult(null, URLsOperationResultCode.AbsentURLs) :
                                          new URLsViewingResult(urls, URLsOperationResultCode.Success);
        }
    }
}
