using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public ICollection<URL> URLs { get; set; } = new List<URL>();
    }
}
