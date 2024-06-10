using GreenMarketBackend.Data;
using GreenMarketBackend.Hubs;
using GreenMarketBackend.Models;
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

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", chatSessionId, userId, content);

            return RedirectToAction("Index", new { chatSessionId });
        }
    }
}
