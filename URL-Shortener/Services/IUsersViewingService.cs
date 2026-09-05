using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public interface IUsersViewingService
    {
        Task<UserViewResult> ViewUserAsync(int id);
    }

    public class UserViewResult
    {
        public User? User { get; private set; }

        public UserOperationResultCode Status { get; private set; }

        public UserViewResult(User? user, UserOperationResultCode status)
        {
            User = user;
            Status = status;
        }
    }
}
