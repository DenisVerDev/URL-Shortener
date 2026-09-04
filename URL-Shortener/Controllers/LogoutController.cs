using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    [Authorize]
    public class LogoutController (IUserSessionService _uss) : Controller
    {
        public async Task<IActionResult> Index()
        {
            await _uss.DeleteCookieSessionAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
