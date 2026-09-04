using Microsoft.AspNetCore.Mvc;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Models;

namespace URL_Shortener.Controllers
{
    public class UrlController (IURLsRepository _urlR, IUsersRepository _ur) : Controller
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
    }
}
