using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    public class AboutController(IPostsViewingService _pvs, IPostsManagementService _pms) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var viewResult = await _pvs.ViewAboutPostAsync();

            var model = new AboutViewModel
            {
                IsAdmin = int.TryParse(User.FindFirstValue(ClaimTypes.Role), out int roleId) && roleId == 2,
                Content = viewResult.Post!.Content
            };

            return View(model);
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<IActionResult> UpdateAboutPost(string content)
        {
            if (content.IsNullOrEmpty())
                return BadRequest();

            var viewResult = await _pvs.ViewAboutPostAsync();

            if (viewResult.Status == PostsOperationResultCode.AbsentPost)
                return NotFound();

            viewResult.Post!.Content = content;

            await _pms.UpdatePostAsync(viewResult.Post);

            return Ok();
        }
    }
}
