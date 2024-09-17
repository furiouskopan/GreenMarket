using GreenMarketBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GreenMarketBackend.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using GreenMarketBackend.Data;
using GreenMarketBackend.Models.ViewModels.ProductViewModels;

namespace GreenMarketBackend.Controllers
    {
        public class HomeController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
            {
                _logger = logger;
                _context = context;
            }

        public async Task<IActionResult> Index()
        {
            // Only select products that are available and NOT featured
            var products = await _context.Products
                .Where(p => p.IsAvailable && p.IsFeatured)
                .Select(p => new FeatureProductViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    MainImageUrl = p.Images.FirstOrDefault(i => i.IsMain != null && i.IsMain).ImageUrl ?? "default-product.png",
                    Price = p.Price,
                    IsFeatured = p.IsFeatured,
                    Description = p.Description,  // Assuming Product model has a Description property
                    Images = p.Images.ToList()
                })
                .ToListAsync();

            return View(products);
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
