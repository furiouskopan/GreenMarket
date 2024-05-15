using Microsoft.AspNetCore.Mvc;

namespace GreenMarketBackend.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
