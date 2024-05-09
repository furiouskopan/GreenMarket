using Microsoft.AspNetCore.Mvc;

namespace GreenMarketBackend.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Products()
        {
            return View();
        }
    }
}
