using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public class RolesRepository (UShortDbContext _dbContext) : IRolesRepository
    {
        public virtual async Task<Role?> FindRoleAsync(int id)
            => await _dbContext.Roles.FindAsync(id);

        public virtual async Task<List<Role>> GetRolesAsync()
            => await _dbContext.Roles.ToListAsync();
    }
}
