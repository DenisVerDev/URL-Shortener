using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;
using URL_Shortener.Models.DTO;
using URL_Shortener.Models.Forms;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    public class HomeController (IURLsRepository _urlR, IURLsViewingService _uvs) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/short/{shortUrlId}")]
        public async Task<IActionResult> RedirectToOriginalURL(string shortUrlId)
        {
            var url = await _urlR.FirstURLAsync(u=>u.ShortURLId == shortUrlId);
            return url is null ? NotFound() : Redirect(url.OriginalURL);
        }

        [HttpGet("/urls")]
        public async Task<IActionResult> PaginateURLs([FromQuery] UrlsFilterFormModel model)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            int totalCount = await _urlR.CountURLsAsync();

            var result = await _uvs.ViewURLsAsync(model.PageIndex, model.PageSize);

            bool parseUserIdResult = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
            bool paresUserRoleResult = int.TryParse(User.FindFirstValue(ClaimTypes.Role), out int roleId);

            var urlDtos = result.URLs?.Select(u => new UrlDTO
            {
                Id = u.Id,
                IsUserAuthority = User.Identity.IsAuthenticated && parseUserIdResult && paresUserRoleResult ? 
                                  userId == u.CreatorId || roleId == 2 : false, // 2 means admin
                OriginalURL = u.OriginalURL,
                ShortURLId = u.ShortURLId
            }).ToList() ?? [];

            var response = new PageUrlsDTO
            {
                Items = urlDtos,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
