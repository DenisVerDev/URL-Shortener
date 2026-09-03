using System.Linq.Expressions;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public interface IURLsRepository
    {
        Task<URL> AddURLAsync(URL url);

        Task<URL> CreateURLAsync(string originalUrl, User creator);

        Task<URL?> FindURLAsync(int id);

        Task<URL?> FindURLAsync(string originalUrl);

        Task<List<URL>> FindURLsAsync(int creatorId);

        Task<List<URL>> FindURLsAsync(Expression<Func<URL, bool>> predicate);

        Task<URL?> FirstURLAsync();

        Task<URL?> FirstURLAsync(int creatorId);

        Task<URL?> FirstURLAsync(Expression<Func<URL, bool>> predicate);

        Task<URL?> LastURLAsync();

        Task<URL?> LastURLAsync(int creatorId);

        Task<URL?> LastURLAsync(Expression<Func<URL, bool>> predicate);

        Task DeleteURLAsync(URL url);

        Task DeleteURLsAsync(IEnumerable<URL> urls);

        Task<bool> AnyURLAsync(Expression<Func<URL, bool>> predicate);
    }
}
