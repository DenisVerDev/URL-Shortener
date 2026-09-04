using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models.Forms
{
    public class RegisterFormModel : LoginFormModel
    {
        [Required(ErrorMessage = "Password is required to create your new account.")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Password must have at least 10 characters and can be up to 20.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Password can contain only English letters and digits.")]
        public override string Password { get; set; } = null!;

        [Required(ErrorMessage = "You must confirm your password before creating new account.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirmation { get; set; } = null!;
    }
}
