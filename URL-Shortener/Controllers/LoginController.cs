using Microsoft.AspNetCore.Mvc;
using URL_Shortener.Models.Forms;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    public class LoginController (IUserManagementService _ums, IUserVerificationService _uvs, IUserSessionService _uss) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginFormModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var result = await _uvs.VerifyUserAsync(model.Login, model.Password);

            switch(result.Status)
            {
                case UserOperationResultCode.AbsentUser:
                    ModelState.AddModelError(nameof(model.Login), "The user does not exist.");
                    return View(model);

                case UserOperationResultCode.VerificationFailure:
                    ModelState.AddModelError(nameof(model.Password), "Incorrect password.");
                    return View(model);

                case UserOperationResultCode.Success:
                    await _uss.CreateCookieSessionAsync(result.User!);
                    break;
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _ums.CreateUserAsync(model.Login, model.Password);

            switch (result.Status)
            {
                case UserOperationResultCode.DuplicateUser:
                    ModelState.AddModelError(nameof(model.Login), "This user already exists.");
                    return View(model);

                case UserOperationResultCode.Success:
                    await _uss.CreateCookieSessionAsync(result.User!);
                    break;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
