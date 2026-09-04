using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IUserManagementService
    {
        Task<UserCreationResult> CreateUserAsync(string login, string password);
    }

    public class UserCreationResult
    {
        public User? User { get; private set; }

        public UserOperationResultCode Status { get; private set; }

        public UserCreationResult(User? user, UserOperationResultCode status)
        {
            User = user;
            Status = status;
        }
    }

    public enum UserOperationResultCode
    {
        Success,
        DuplicateUser,
        AbsentUser,
        VerificationFailure
    }
}
