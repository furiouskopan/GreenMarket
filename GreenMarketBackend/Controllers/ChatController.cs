using GreenMarketBackend.Data;
using GreenMarketBackend.Hubs;
using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels.CartViewModels;
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
        public async Task<IActionResult> SendMessage(int chatSessionId, string content, string toUser)
        {
            try
            {
                Console.WriteLine($"SendMessage called with chatSessionId: {chatSessionId}, content: '{content}', toUser: '{toUser}'");

                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine("Message content is empty");
                    return BadRequest("Message content cannot be empty.");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    Console.WriteLine("User not authenticated");
                    return BadRequest("User not authenticated.");
                }

                var sender = await _context.Users.FindAsync(userId);
                if (sender == null)
                {
                    Console.WriteLine("Sender not found");
                    return BadRequest("Sender not found.");
                }

                var message = new Message
                {
                    ChatSessionId = chatSessionId,
                    SenderId = userId,
                    Content = content,
                    Timestamp = DateTime.Now,
                    Sender = sender
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                Console.WriteLine("Message saved successfully");

                var recipient = await _context.Users.FirstOrDefaultAsync(u => u.Email == toUser);
                if (recipient != null)
                {
                    await _hubContext.Clients.User(recipient.Id).SendAsync("ReceiveMessage", sender.UserName, content);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
    }