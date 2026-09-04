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
    public class HomeController (IURLsRepository _urlR, IURLsManagementService _ums, IURLsViewingService _uvs) : Controller
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
        public async Task<IActionResult> PaginateURLs([FromBody] UrlsFilterFormModel model)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            int totalCount = await _urlR.CountURLsAsync();

            var result = await _uvs.ViewURLsAsync(model.PageIndex, model.PageSize);

            var urlDtos = result.URLs?.Select(u => new UrlDTO
            {
                Id = u.Id,
                CreatorId = u.CreatorId,
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        [HttpPost("/short")]
        public async Task<IActionResult> ShortenURL([FromBody] ShortenUrlFormModel model)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var claimUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!int.TryParse(claimUserId, out int userId))
                return Unauthorized();

            var result = await _ums.CreateURLAsync(model.URL, userId);

            switch (result.Status)
            {
                case URLsOperationResultCode.AbsentUser:
                    return Forbid();

                case URLsOperationResultCode.DuplicateURL:
                    var url =  await _urlR.FindURLAsync(model.URL);
                    return url is null ? Problem("Problem occured while searching for the existing url.") : Ok(url.ShortURLId);

                default:
                    return Ok(result.URL!.ShortURLId);
            }
        }
    }
}
