using GreenMarketBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

public class ContactController : Controller
{
    private readonly EmailService _emailService;

    public ContactController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(ContactFormModel model)
    {
        if (ModelState.IsValid)
        {
            var message = $"Name: {model.Name}<br>Email: {model.Email}<br>Message: {model.Message}";
            await _emailService.SendEmailAsync("packaras@gmail.com", "Zelkite", message);
            ViewBag.Message = "Message sent successfully!";
            return View();
        }
        return View(model);
    }
    public IActionResult ContactConfirmation()
    {
        return View();
    }
}
