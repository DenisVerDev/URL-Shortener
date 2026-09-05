using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;

namespace URL_Shortener.Controllers
{
    public class AboutController (IPostsRepository _pr) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var aboutPost = await _pr.FindAboutPostAsync();

            var model = new AboutViewModel
            {
                IsAdmin = int.TryParse(User.FindFirstValue(ClaimTypes.Role), out int roleId) && roleId == 2,
                Content = aboutPost?.Content
            };

            return View(model);
        }
    }
}
