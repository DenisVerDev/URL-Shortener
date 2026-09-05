using System.Linq.Expressions;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> AddUserAsync(User user);

        Task<User?> CreateUserAsync(string login, string password);

        Task<User?> FindUserAsync(int id);

        Task<User?> FindUserAsync(string login);

        Task<List<User>> FindUsersAsync(int roleId);

        Task<List<User>> FindUsersAsync(Expression<Func<User, bool>> predicate);

        Task<User?> FirstUserAsync();

        Task<User?> FirstUserAsync(Expression<Func<User, bool>> predicate);

        Task<User?> LastUserAsync();

        Task<User?> LastUserAsync(Expression<Func<User, bool>> predicate);

        Task<bool> AnyUserAsync(Expression<Func<User, bool>> predicate);
    }
}
