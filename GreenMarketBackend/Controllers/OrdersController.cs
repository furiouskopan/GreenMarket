using Microsoft.AspNetCore.Mvc;

namespace GreenMarketBackend.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
