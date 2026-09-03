using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IUserVerificationService
    {
        Task<UserVerificationResult> VerifyUserAsync(string login, string password);
    }

    public class UserVerificationResult
    {
        public User? User { get; private set; }

        public UserVerificationResultCode Status { get; private set; }

        public UserVerificationResult(User? user, UserVerificationResultCode status)
        {
            User = user;
            Status = status;
        }
    }

    public enum UserVerificationResultCode
    {
        Success,
        AbsentUser,
        VerificationFailure
    }
}
