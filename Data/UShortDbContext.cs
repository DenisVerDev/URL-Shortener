using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data
{
    public class UShortDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<URL> URLs { get; set; }

        public UShortDbContext(DbContextOptions<UShortDbContext> options) : base(options)
        {}
    }
}
