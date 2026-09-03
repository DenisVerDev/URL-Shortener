using URL_Shortener.Data.Models;

namespace URL_Shortener.Data.Repositories
{
    public interface IRolesRepository
    {
        Task<Role?> FindRoleAsync(int id);

        Task<List<Role>> GetRolesAsync();
    }
}
