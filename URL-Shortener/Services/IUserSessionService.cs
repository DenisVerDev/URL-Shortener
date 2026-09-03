using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IUserSessionService
    {
        Task CreateCookieSessionAsync(User user);

        Task DeleteCookieSessionAsync();
    }
}
