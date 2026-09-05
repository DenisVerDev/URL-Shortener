using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URL_Shortener.Data.Models;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;
using URL_Shortener.Models.Forms;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    // Move repository logic into service layer

    [Authorize]
    public class UrlController (IUsersRepository _ur, IURLsManagementService _ums, IURLsViewingService _uvs) : Controller
    {
        public async Task<IActionResult> Index(int id)
        {
            var result = await _uvs.ViewURLAsync(id);

            if(result.Status == URLsOperationResultCode.AbsentURL)
                return NotFound();

            var creator = await _ur.FindUserAsync(result.URL!.CreatorId);

            if(creator is null)
                return NotFound();

            var model = new UrlViewModel
            {
                OriginalURL = result.URL.OriginalURL,
                ShortURLId = result.URL.ShortURLId,
                Creator = creator.Login,
                CreationDate = result.URL.CreationDate
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
                    var viewResult = await _uvs.ViewURLAsync(model.URL);
                    return viewResult.Status == URLsOperationResultCode.Success ? Ok(viewResult.URL!.ShortURLId) :
                                                                                  Problem("Problem occured while searching for the existing url.");

                default:
                    return Ok(result.URL!.ShortURLId);
            }
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/delete/personal/{id}")]
        public async Task<IActionResult> DeletePersonalURL(int id)
        {
            var viewResult = await _uvs.ViewURLAsync(id);

            if (viewResult.Status == URLsOperationResultCode.AbsentURL)
                return NotFound();

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                return BadRequest();

            if (userId != viewResult.URL!.CreatorId)
                return Forbid();

            var result = await _ums.DeleteURLAsync(viewResult.URL);

            return Ok(result);
        }

        [Authorize(Roles = "2")]
        [HttpDelete("/delete/{id}")]
        public async Task<IActionResult> DeleteURL(int id)
        {
            var viewResult = await _uvs.ViewURLAsync(id);

            if (viewResult.Status == URLsOperationResultCode.AbsentURL)
                return NotFound();

            var result = await _ums.DeleteURLAsync(viewResult.URL!);

            return Ok(result);
        }
    }
}
