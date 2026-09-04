using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models.Forms
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "Login is required to sign in into your account.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Login must have at least 6 characters and can be up to 20.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Login can contain only English letters and digits.")]

        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Password is required to sign in into your account.")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Password must have at least 10 characters and can be up to 20.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Password can contain only English letters and digits.")]
        public string Password { get; set; } = null!;
    }
}
