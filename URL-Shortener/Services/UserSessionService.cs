using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using URL_Shortener.Data.Models;

namespace URL_Shortener.Services
{
    public class UserSessionService (IHttpContextAccessor _hca) : IUserSessionService
    {
        public async Task CreateCookieSessionAsync(User user)
        {
            if(user is null)
                throw new ArgumentNullException($"{nameof(UserSessionService)} cannot creake auth cookie with null user!");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _hca.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task DeleteCookieSessionAsync()
            => await _hca.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
