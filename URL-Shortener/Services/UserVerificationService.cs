using Microsoft.IdentityModel.Tokens;
using URL_Shortener.Data.Repositories;
using bc = BCrypt.Net.BCrypt;

namespace URL_Shortener.Services
{
    public class UserVerificationService (IUsersRepository _ur) : IUserVerificationService
    {
        public virtual async Task<UserVerificationResult> VerifyUserAsync(string login, string password)
        {
            if (login.IsNullOrEmpty() || password.IsNullOrEmpty())
                throw new ArgumentException($"{nameof(UserVerificationService)} cannot verify with null or empty data!");

            var user = await _ur.FindUserAsync(login);

            if (user is null)
                return new UserVerificationResult(null, UserOperationResultCode.AbsentUser);

            if (!bc.Verify(password, user.PasswordHash))
                return new UserVerificationResult(null, UserOperationResultCode.VerificationFailure);

            return new UserVerificationResult(user, UserOperationResultCode.Success);
        }
    }
}
