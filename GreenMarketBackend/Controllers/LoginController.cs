using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using GreenMarketBackend.Models;
using System.Security.Claims;
using GreenMarketBackend.Models.ViewModels.AccountViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace GreenMarketBackend.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, EmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;   
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
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                // Log invalid model state to see what went wrong
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"ModelState Error: Key = {state.Key}, Error = {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            // Check if the user exists
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Log the error to track the issue
                Console.WriteLine($"Login failed: No user found with email {model.Email}");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Try signing in the user
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Successful login
                Console.WriteLine($"Login succeeded for user {user.UserName}");
                return LocalRedirect(returnUrl);
            }
            //else if (result.IsLockedOut)
            //{
            //    // Log if account is locked out
            //    Console.WriteLine($"Login failed: User {user.UserName} is locked out.");
            //    ModelState.AddModelError(string.Empty, "Account is locked out.");
            //}
            //else if (result.IsNotAllowed)
            //{
            //    // Log if login is not allowed
            //    Console.WriteLine($"Login failed: User {user.UserName} is not allowed to log in.");
            //    ModelState.AddModelError(string.Empty, "You are not allowed to log in.");
            //}
            //else if (result.RequiresTwoFactor)
            //{
            //    // Log if two-factor authentication is required
            //    Console.WriteLine($"Login failed: Two-factor authentication required for user {user.UserName}.");
            //    return RedirectToAction(nameof(TwoFactorAuthentication));
            //}
            //else
            //{
            //    // Log general failure
            //    Console.WriteLine($"Login failed: Invalid login attempt for user {user.UserName}.");
            //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //}

            return View(model);
        }


        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Login", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Index");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Index)); // Optionally, handle lockout scenarios
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var model = new ExternalLoginViewModel { Email = email };

                // Pass the model to the view to complete the registration
                return View("ExternalLogin", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider.
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Prevent revealing user existence
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            await _emailService.SendEmailAsync(model.Email, "Reset Password",
                $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");

            return RedirectToAction("ForgotPasswordConfirmation");
        }
    }
}
