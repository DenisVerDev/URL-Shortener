using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;
using URL_Shortener.Models.Forms;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    public class HomeController (IURLsRepository _urlR, IURLsManagementService _ums) : Controller
    {
        public IActionResult Index()
        {
            return View();
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
        [HttpPost("short")]
        public async Task<IActionResult> ShortenURL(ShortenUrlFormModel model)
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
                    var url = await _urlR.FindURLAsync(model.URL);
                    return Conflict(new
                    {
                        message = "This url was already shortened.",
                        url
                    });

                default:
                    return Ok(result.URL);
            }
        }
    }
}
