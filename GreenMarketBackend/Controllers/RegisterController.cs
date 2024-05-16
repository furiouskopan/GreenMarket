using Microsoft.AspNetCore.Mvc;

namespace GreenMarketBackend.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
