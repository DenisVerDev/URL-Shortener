using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models.Forms
{
    public class RegisterFormModel : LoginFormModel
    {
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirmation { get; set; } = null!;
    }
}
