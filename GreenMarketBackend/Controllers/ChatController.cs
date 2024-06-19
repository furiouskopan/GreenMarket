using GreenMarketBackend.Data;
using GreenMarketBackend.Hubs;
using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels.ChatViewModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? sessionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chatSessions = await _context.ChatSessions
                .Where(cs => cs.User1Id == userId || cs.User2Id == userId)
                .Include(cs => cs.Messages)
                .ThenInclude(m => m.Sender)
                .Include(cs => cs.User1)
                .Include(cs => cs.User2)
                .ToListAsync();

            var viewModel = new ChatViewModel
            {
                ChatSessions = chatSessions,
                SelectedChatSessionId = sessionId ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StartChatByUsername([FromBody] StartChatRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            if (recipient == null)
            {
                return BadRequest("Recipient not found.");
            }

            var existingSession = await _context.ChatSessions
                .FirstOrDefaultAsync(cs =>
                    (cs.User1Id == userId && cs.User2Id == recipient.Id) ||
                    (cs.User1Id == recipient.Id && cs.User2Id == userId));

            if (existingSession == null)
            {
                var chatSession = new ChatSession
                {
                    User1Id = userId,
                    User2Id = recipient.Id
                };
                _context.ChatSessions.Add(chatSession);
                await _context.SaveChangesAsync();
                return Json(new { sessionId = chatSession.Id });
            }

            return Json(new { sessionId = existingSession.Id });
        }

        public class StartChatRequest
        {
            public string Username { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                return BadRequest("Message content cannot be empty.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User not authenticated.");
            }

            var sender = await _context.Users.FindAsync(userId);
            if (sender == null)
            {
                return BadRequest("Sender not found.");
            }

            var chatSession = await _context.ChatSessions.FindAsync(model.ChatSessionId);
            if (chatSession == null)
            {
                return BadRequest("Chat session not found.");
            }

            var message = new Message
            {
                ChatSessionId = model.ChatSessionId,
                SenderId = userId,
                Content = model.Content,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var recipientId = (chatSession.User1Id == userId) ? chatSession.User2Id : chatSession.User1Id;
            await _hubContext.Clients.User(recipientId).SendAsync("ReceiveMessage", model.ChatSessionId, sender.UserName, model.Content);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChatSession([FromBody] DeleteChatSessionRequest request)
        {
            var chatSession = await _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => cs.Id == request.ChatSessionId);

            if (chatSession == null)
            {
                return NotFound();
            }

            _context.Messages.RemoveRange(chatSession.Messages);
            _context.ChatSessions.Remove(chatSession);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public class DeleteChatSessionRequest
        {
            public int ChatSessionId { get; set; }
        }
    }
}
