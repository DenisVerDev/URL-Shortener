using Microsoft.IdentityModel.Tokens;
using URL_Shortener.Data.Repositories;
using bc = BCrypt.Net.BCrypt;

namespace URL_Shortener.Services
{
    public class UserVerificationService (IUsersRepository _ur) : IUserVerification
    {
        public virtual async Task<UserVerificationResult> VerifyUserAsync(string login, string password)
        {
            if (login.IsNullOrEmpty() || password.IsNullOrEmpty())
                throw new ArgumentException($"{nameof(UserVerificationService)} cannot verify with null or empty data!");

            var user = await _ur.FindUserAsync(login);

            if (user is null)
                return new UserVerificationResult(null, UserVerificationResultCode.AbsentUser);

            if (!bc.Verify(password, user.PasswordHash))
                return new UserVerificationResult(null, UserVerificationResultCode.VerificationFailure);

            return new UserVerificationResult(user, UserVerificationResultCode.Success);
        }
    }
}
