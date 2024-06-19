using GreenMarketBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GreenMarketBackend.Models.ViewModels;

namespace GreenMarketBackend.Controllers
    {
        public class HomeController : Controller
        {
            private readonly ILogger<HomeController> _logger;

            public HomeController(ILogger<HomeController> logger)
            {
                _logger = logger;
            }

            public IActionResult Index()
            {
                return View();
            }
            public IActionResult About()
            {
                return View();
            }
            public async Task<IActionResult> Contact()
            {
                var model = new ContactViewModel(); 
                return View(model);
            }
            [HttpPost]
            public async Task<IActionResult> Contact(ContactViewModel model)
            {
                if (ModelState.IsValid)
                {
                    // Send email logic here
                    return RedirectToAction("ContactConfirmation");
                }
                return View(model);
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
