using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GreenMarketBackend.Models;
using System.Threading.Tasks;
using GreenMarketBackend.Models.ViewModels;

namespace GreenMarketBackend.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/"); // If returnUrl is null, redirect to the home page

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }
    }
}
