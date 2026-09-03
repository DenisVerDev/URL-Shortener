using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public class UsersRepository (UShortDbContext _dbContext) : IUsersRepository
    {
        public virtual async Task<User?> FindUserAsync(int id)
            => await _dbContext.Users.FindAsync(id);

        public virtual async Task<User?> FindUserAsync(string login)
            => await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);

        public virtual async Task<User?> FindUserAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.FirstOrDefaultAsync(predicate);

        public virtual async Task<List<User>> FindUsersAsync(int roleId)
            => await _dbContext.Users.Where(u => u.Role.Id == roleId).ToListAsync();

        public virtual async Task<List<User>> FindUsersAsync(DateTime registrationDate)
           => await _dbContext.Users.Where(u => u.RegistrationDate == registrationDate).ToListAsync();

        public virtual async Task<List<User>> FindUsersAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.Where(predicate).ToListAsync();

        public virtual async Task<User?> FirstUserAsync()
            => await _dbContext.Users.FirstOrDefaultAsync();

        public virtual async Task<User?> LastUserAsync()
            => await _dbContext.Users.LastOrDefaultAsync();

        public virtual async Task<User?> LastUserAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.LastOrDefaultAsync(predicate);

        public virtual async Task<bool> AnyUserAsync(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.AnyAsync(predicate);
    }
}
