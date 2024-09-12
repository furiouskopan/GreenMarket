﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;


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

            // Include Images along with Category
            IQueryable<Product> productsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images); // Make sure images are included in the query

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
                // Ensure at least 3 images are provided
                if (model.ImageFiles.Count < 3)
                {
                    ModelState.AddModelError("", "At least 3 images are required.");
                    ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
                    return View(model);
                }

                var user = await _userManager.GetUserAsync(User);
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    Pesticides = model.Pesticides,
                    Origin = model.Origin,
                    CreatedDate = DateTime.UtcNow,
                    HarvestDate = model.HarvestDate,
                    CreatedByUserId = user.Id,
                    CategoryId = model.CategoryId,
                    IsAvailable = true
                };

                for (int i = 0; i < model.ImageFiles.Count; i++)
                {
                    var imageFile = model.ImageFiles[i];
                    var imageUrl = await SaveImageAsync(imageFile); // SaveImageAsync should store the image and return the URL

                    var productImage = new ProductImage
                    {
                        ImageUrl = imageUrl,
                        IsMain = i == model.MainImageIndex // Mark the image as the main image if it's the selected one
                    };

                    product.Images.Add(productImage);
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
            return View(model);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            // Ensure the directory exists
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            // Generate a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Load the image and resize it
            using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
            {
                // Resize the image (example: resize to a width of 800px, maintain aspect ratio)
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(800, 600), // Adjust size as needed
                    Mode = ResizeMode.Crop
                }));

                // Save the resized image to the server
                await image.SaveAsync(filePath);
            }

            // Return the relative path to the image
            return Path.Combine("images", uniqueFileName);
        }

        // Handles file upload for product images
        //private string UploadedFile(ProductViewModel model)
        //{
        //    string uniqueFileName = null;

        //    if (model.ImageFile != null)
        //    {
        //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.ImageFile.CopyTo(fileStream);
        //        }
        //    }

        //    return uniqueFileName;
        //}

        //// Displays the delete confirmation page
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// Handles the POST request to delete a product
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Console.WriteLine($"Received product ID for deletion: {id}"); // Logging the received ID

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                Console.WriteLine($"Product with ID: {id} not found."); // Log when product is not found
                return Json(new { success = false, message = "Product not found." });
            }

            var isInCarts = _context.CartItems.Any(ci => ci.ProductId == id);

            if (isInCarts)
            {
                product.IsAvailable = false;
                _context.Products.Update(product);
            }
            else
            {
                _context.Products.Remove(product);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while saving changes: {ex.Message}"); // Log any errors during save
                return Json(new { success = false, message = "An error occurred while saving changes." });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminHardDelete(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Reviews)
                .Include(p => p.CartItems)
                .Include(p => p.OrderItens) // Include OrderItems to ensure deletion
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                Console.WriteLine($"Product with ID {productId} not found.");
                return NotFound();
            }

            // Remove associated CartItems
            if (product.CartItems != null && product.CartItems.Any())
            {
                _context.CartItems.RemoveRange(product.CartItems);
                Console.WriteLine($"Cart items for product {productId} deleted.");
            }

            // Remove associated Reviews
            if (product.Reviews != null && product.Reviews.Any())
            {
                _context.Reviews.RemoveRange(product.Reviews);
                Console.WriteLine($"Reviews for product {productId} deleted.");
            }

            // Remove associated OrderItems
            if (product.OrderItens != null && product.OrderItens.Any())
            {
                _context.OrderItems.RemoveRange(product.OrderItens);
                Console.WriteLine($"Order items for product {productId} deleted.");
            }

            // Remove the product itself
            _context.Products.Remove(product);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine($"Product with ID {productId} and all related entities successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting product with ID {productId}: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }

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
                .Include(p => p.Images)  // Include the images
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
                Uploader = product.CreatedByUser,
                Images = product.Images.ToList(),  // Add images to the view model
                //MainImageUrl = product.MainImageUrl  // Set the main image URL
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

            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == id);
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
                ExistingImageUrls = product.Images.Select(i => i.ImageUrl).ToList(),
                Pesticides = product.Pesticides,
                Origin = product.Origin,
                HarvestDate = product.HarvestDate,
                CategoryId = product.CategoryId,
                CreatedDate = product.CreatedDate,
                MainImageIndex = product.Images.ToList().FindIndex(i => i.IsMain)
            };

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(viewModel);
        }


        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model, List<string> RemoveImages)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    // Update product details
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.StockQuantity = model.StockQuantity;
                    product.Pesticides = model.Pesticides;
                    product.Origin = model.Origin;
                    product.HarvestDate = model.HarvestDate;
                    product.CategoryId = model.CategoryId;

                    // Remove selected images
                    if (RemoveImages != null && RemoveImages.Any())
                    {
                        foreach (var imageUrl in RemoveImages)
                        {
                            var imageToRemove = product.Images.FirstOrDefault(i => i.ImageUrl == imageUrl);
                            if (imageToRemove != null)
                            {
                                _context.ProductImages.Remove(imageToRemove);
                            }
                        }
                    }

                    // Handle new image uploads
                    if (model.ImageFiles != null && model.ImageFiles.Count > 0)
                    {
                        foreach (var imageFile in model.ImageFiles.Take(5)) // Limit to 5 images
                        {
                            var imagePath = await SaveImageAsync(imageFile);
                            product.Images.Add(new ProductImage { ImageUrl = imagePath });
                        }
                    }

                    // Update the main image index
                    product.MainImageIndex = model.MainImageIndex;

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
