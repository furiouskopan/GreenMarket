using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using GreenMarketBackend.Models.ViewModels.ProductViewModels;

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

        // Displays a list of products with optional filtering by category and sorting
        public async Task<IActionResult> Index(int? parentCategoryId, int? childCategoryId, string sortOrder, string search)
        {
            var parentCategories = await _context.Categories.Where(c => c.ParentCategoryId == null).ToListAsync();
            var childCategories = Enumerable.Empty<Category>();
            IQueryable<Product> productsQuery = _context.Products.Include(p => p.Category);

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(search));
            }

            if (childCategoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == childCategoryId);
                childCategories = await _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
            }
            else if (parentCategoryId.HasValue)
            {
                childCategories = await _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
                productsQuery = productsQuery.Where(p => p.Category.ParentCategoryId == parentCategoryId);
            }

            switch (sortOrder)
            {
                case "asc": productsQuery = productsQuery.OrderBy(p => p.Price); break;
                case "desc": productsQuery = productsQuery.OrderByDescending(p => p.Price); break;
            }

            var products = await productsQuery.ToListAsync();

            var viewModel = new ProductFilterViewModel
            {
                ParentCategories = parentCategories,
                ChildCategories = childCategories,
                Products = products,
                SelectedParentCategoryId = parentCategoryId,
                SelectedChildCategoryId = childCategoryId,
                Search = search
            };

            return View(viewModel);
        }

        // Returns the view for creating a new product
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // Handles the POST request to create a new product
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
                    CategoryId = model.CategoryId
                };

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            return View(model);
        }

        // Handles file upload for product images
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

        // Displays the delete confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Handles the POST request to delete a product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Displays the details of a specific product, including reviews
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
                ProductId = product.ProductId,  
                Product = product,
                Reviews = product.Reviews.ToList(),
                NewReview = new ReviewSubmissionViewModel { ProductId = product.ProductId },
                Uploader = product.CreatedByUser
            };

            return View(viewModel);
        }


        // GET: Products/MyProducts
        public async Task<IActionResult> MyProducts()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Where(p => p.CreatedByUserId == user.Id)
                .ToListAsync();

            return View(products);
        }

        // Handles the POST request to add a review to a product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(AddReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload the product and reviews for the view if validation fails
                var product = await _context.Products
                    .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                    .Include(p => p.CreatedByUser)
                    .FirstOrDefaultAsync(m => m.ProductId == model.ProductId);

                if (product == null)
                {
                    return NotFound();
                }

                var viewModel = new ProductDetailsViewModel
                {
                    Product = product,
                    Reviews = product.Reviews.ToList(),
                    NewReview = new ReviewSubmissionViewModel { ProductId = product.ProductId },
                    Uploader = product.CreatedByUser
                };

                return View("Details", viewModel);
            }

            var user = await _userManager.GetUserAsync(User);

            var review = new Review
            {
                ProductId = model.ProductId,
                UserId = user.Id,
                Comment = model.Comment,
                Rating = model.Rating,
                CreatedDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Update the product's average rating and review count
            var updatedProduct = await _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == model.ProductId);

            updatedProduct.AverageRating = updatedProduct.Reviews.Average(r => r.Rating);
            updatedProduct.ReviewCount = updatedProduct.Reviews.Count;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = model.ProductId });
        }
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductEditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageURL = product.ImageURL,
                Pesticides = product.Pesticides,
                Origin = product.Origin,
                HarvestDate = product.HarvestDate,
                CategoryId = product.CategoryId,
                CreatedDate = product.CreatedDate,
            };

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(viewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.StockQuantity = model.StockQuantity;
                    product.ImageURL = model.ImageURL;
                    product.Pesticides = model.Pesticides;
                    product.Origin = model.Origin;
                    product.HarvestDate = model.HarvestDate;
                    product.CategoryId = model.CategoryId;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyProducts));
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
            return View(model);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
