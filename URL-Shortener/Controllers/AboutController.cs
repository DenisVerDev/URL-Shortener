using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;

namespace URL_Shortener.Controllers
{
    public class AboutController(IPostsRepository _pr) : Controller
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

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<IActionResult> UpdateAboutPost(string content)
        {
            if (content.IsNullOrEmpty())
                return BadRequest();

            var aboutPost = await _pr.FindAboutPostAsync();

            if (aboutPost is null)
                return NotFound();

            aboutPost.Content = content;

            await _pr.UpdatePostAsync(aboutPost);

            return Ok();
        }
    }
}
