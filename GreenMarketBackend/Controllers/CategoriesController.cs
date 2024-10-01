using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GreenMarketBackend.Controllers
{
    [Route("Categories")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetChildCategories")]
        public async Task<JsonResult> GetChildCategories(int parentCategoryId)
        {
            var childCategories = await _context.Categories
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .Select(c => new
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return Json(childCategories);
        }
    }
}
