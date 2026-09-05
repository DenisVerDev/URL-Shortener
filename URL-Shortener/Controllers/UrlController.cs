using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;
using URL_Shortener.Models.Forms;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    // Move repository logic into service layer
    // Move DeleteURL and ShortenURL methods here

    [Authorize]
    public class UrlController (IURLsRepository _urlR, IUsersRepository _ur, IURLsManagementService _ums) : Controller
    {
        public async Task<IActionResult> Index(int id)
        {
            var url = await _urlR.FindURLAsync(id);

            if(url is null)
                return NotFound();

            var creator = await _ur.FindUserAsync(url.CreatorId);

            if(creator is null)
                return NotFound();

            var model = new UrlViewModel
            {
                OriginalURL = url.OriginalURL,
                ShortURLId = url.ShortURLId,
                Creator = creator.Login,
                CreationDate = url.CreationDate
            };

            return View(model);
        }

        [HttpPost("/short")]
        public async Task<IActionResult> ShortenURL([FromBody] ShortenUrlFormModel model)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var claimUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(claimUserId, out int userId))
                return Unauthorized();

            var result = await _ums.CreateURLAsync(model.URL, userId);

            switch (result.Status)
            {
                case URLsOperationResultCode.AbsentUser:
                    return Forbid();

                case URLsOperationResultCode.DuplicateURL:
                    var url = await _urlR.FindURLAsync(model.URL);
                    return url is null ? Problem("Problem occured while searching for the existing url.") : Ok(url.ShortURLId);

                default:
                    return Ok(result.URL!.ShortURLId);
            }
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/delete/personal/{id}")]
        public async Task<IActionResult> DeletePersonalURL(int id)
        {
            var url = await _urlR.FindURLAsync(id);

            if (url is null)
                return NotFound();

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                return BadRequest();

            if (userId != url.CreatorId)
                return Forbid();

            var result = await _ums.DeleteURLAsync(url);

            return Ok(result);
        }

        [Authorize(Roles = "2")]
        [HttpDelete("/delete/{id}")]
        public async Task<IActionResult> DeleteURL(int id)
        {
            var url = await _urlR.FindURLAsync(id);

            if (url is null)
                return NotFound();

            var result = await _ums.DeleteURLAsync(url);

            return Ok(result);
        }
    }
}
