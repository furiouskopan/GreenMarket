using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace GreenMarketBackend.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? parentCategoryId, int? childCategoryId, string sortOrder)
        {
            var parentCategories = await _context.Categories.Where(c => c.ParentCategoryId == null).ToListAsync();
            var childCategories = Enumerable.Empty<Category>();

            // Start with a queryable to allow for more efficient querying.
            IQueryable<Product> productsQuery = _context.Products.Include(p => p.Category);

            // Apply child category filter if specified
            if (childCategoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == childCategoryId);
                childCategories = await _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync(); // Fetch child categories based on parent category if available
            }
            else if (parentCategoryId.HasValue)
            {
                // Apply parent category filter if child category is not specified
                childCategories = await _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
                productsQuery = productsQuery.Where(p => p.Category.ParentCategoryId == parentCategoryId);
            }

            switch (sortOrder)
            {
                case "asc":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "desc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
            }

            var products = await productsQuery.ToListAsync();

            var viewModel = new ProductFilterViewModel
            {
                ParentCategories = parentCategories,
                ChildCategories = childCategories,
                Products = products,
                SelectedParentCategoryId = parentCategoryId,
                SelectedChildCategoryId = childCategoryId
            };

            return View(viewModel);
        }


        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                var user = await _userManager.GetUserAsync(User);
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    ImageURL = uniqueFileName,
                    Pesticides = model.Pesticides,
                    Origin = model.Origin,
                    CreatedDate = DateTime.UtcNow,
                    HarvestDate = model.HarvestDate,
                    CreatedByUserId = user.Id,
                    CategoryId = model.CategoryId,
                };

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the index action or dashboard
            }
            return View(model);
        }
        private string UploadedFile(ProductViewModel model)
        {
            string uniqueFileName = null;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {   
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .Include(p => p.CreatedByUser)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = product.Reviews.ToList(),
                NewReview = new Review(),
                Uploader = product.CreatedByUser
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(ProductDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var review = new Review
                {
                    ProductId = model.NewReview.ProductId,
                    UserId = user.Id,
                    Comment = model.NewReview.Comment,
                    Rating = model.NewReview.Rating,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // Update the product's average rating and review count
                var product = await _context.Products
                    .Include(p => p.Reviews)
                    .FirstOrDefaultAsync(p => p.ProductId == model.NewReview.ProductId);

                product.AverageRating = product.Reviews.Average(r => r.Rating);
                product.ReviewCount = product.Reviews.Count;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = model.NewReview.ProductId });
            }

            return View("Details", model);
        }
    }
}
