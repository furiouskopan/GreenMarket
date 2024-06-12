﻿using GreenMarketBackend.Data;
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

        public async Task<IActionResult> Index(int chatSessionId)
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ChatSessionId == chatSessionId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var viewModel = new ChatViewModel
            {
                Messages = messages,
                ChatSessionId = chatSessionId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int chatSessionId, string recipient, string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sender = await _context.Users.FindAsync(userId);
            var recipientUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == recipient);

            if (recipientUser == null)
            {
                // Handle recipient not found
                return RedirectToAction("Index", new { chatSessionId });
            }

            var message = new Message
            {
                ChatSessionId = chatSessionId,
                SenderId = userId,
                Content = content,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(recipientUser.Id).SendAsync("ReceiveMessage", sender.UserName, content, chatSessionId);

            return RedirectToAction("Index", new { chatSessionId });
        }
    }
}
