using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public Role Role { get; set; }

        public List<URL> URLs { get; set; }
    }
}
