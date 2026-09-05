
using Microsoft.IdentityModel.Tokens;
using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services
{
    public class UserManagementService (IUsersRepository _ur) : IUserManagementService
    {
        public async Task<UserCreationResult> CreateUserAsync(string login, string password)
        {
            if (login.IsNullOrEmpty() || password.IsNullOrEmpty())
                throw new ArgumentException($"{nameof(UserManagementService)} cannot create new user with null or empty data!");

            if (await _ur.AnyUserAsync(u => u.Login == login))
                return new UserCreationResult(null, UserOperationResultCode.DuplicateUser);

            var user = await _ur.CreateUserAsync(login, password);

            return new UserCreationResult(user, UserOperationResultCode.Success);
        }
    }
}
