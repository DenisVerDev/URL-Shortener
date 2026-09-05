using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Data
{
    public class UShortDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<URL> URLs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public UShortDbContext(DbContextOptions<UShortDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRoles(modelBuilder);
            ConfigureUsers(modelBuilder);
            ConfigureURLs(modelBuilder);
            ConfigurePosts(modelBuilder);
        }

        private static void ConfigureRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(r => r.Name)
                      .IsUnique();

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_Role_Name_NotEmpty", "LEN(TRIM([Name])) > 0");
                });
            });
        }

        private static void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Login)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(x => x.Login)
                    .IsUnique();

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_User_Login_Length",
                        "LEN([Login]) >= 6");

                    table.HasCheckConstraint(
                        "CK_User_Login_Characters",
                        "[Login] COLLATE Latin1_General_100_BIN2 " +
                        "NOT LIKE '%[^A-Za-z0-9]%'");
                });

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.Property(x => x.RegistrationDate)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasOne(x => x.Role)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.RoleId)
                    .IsRequired();
            });
        }

        private static void ConfigureURLs(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<URL>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.OriginalURL)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.HasIndex(x => x.OriginalURL)
                      .IsUnique();

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_URL_OriginalURL_NotEmpty",
                        "LEN(TRIM([OriginalURL])) > 0");

                    table.HasCheckConstraint(
                        "CK_URL_ShortUrlId_NotEmpty",
                        "LEN(TRIM([ShortURLId])) > 0");
                });

                entity.Property(x => x.ShortURLId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.ShortURLId)
                    .IsUnique();

                entity.Property(x => x.CreationDate)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasOne(x => x.Creator)
                    .WithMany(x => x.URLs)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();
            });
        }

        public static void ConfigurePosts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Content)
                      .IsRequired();
            });
        }
    }
}
