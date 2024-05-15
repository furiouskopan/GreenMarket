using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GreenMarketBackend.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,StockQuantity,ImageURL,CreatedDate,HarvestDate,CategoryId,CreatedByUserId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
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
    }
}
