using URL_Shortener.Data;
using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services
{
    public class UsersViewingService(IUsersRepository _ur) : IUsersViewingService
    {
        public async Task<UserViewResult> ViewUserAsync(int id)
        {
            var user = await _ur.FindUserAsync(id);
            var status = user is null ? UserOperationResultCode.AbsentUser : UserOperationResultCode.Success;

            return new UserViewResult(user, status);
        }
    }
}
