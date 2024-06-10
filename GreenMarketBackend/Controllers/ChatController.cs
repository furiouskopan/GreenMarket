using GreenMarketBackend.Data;
using GreenMarketBackend.Hubs;
using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels.CartViewModels;
using GreenMarketBackend.Models.ViewModels.ChatViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GreenMarketBackend.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index()
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var viewModel = new ChatViewModel
            {
                Messages = messages
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int chatSessionId, string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var message = new Message
            {
                ChatSessionId = chatSessionId,
                SenderId = userId,
                Content = content,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var sender = await _context.Users.FindAsync(userId);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", sender.UserName, content);

            return RedirectToAction("Index", new { chatSessionId });
        }
    }
}
