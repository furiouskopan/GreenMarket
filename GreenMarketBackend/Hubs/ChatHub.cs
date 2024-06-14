using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GreenMarketBackend.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        private static readonly Dictionary<string, string> _connections = new();

        public override Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name;
            if (userName != null)
            {
                _connections[Context.ConnectionId] = userName;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message, string toUser)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message content cannot be emptyHUB", nameof(message));
            }

            var connectionId = _connections.FirstOrDefault(x => x.Value == toUser).Key;
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                Console.WriteLine("Recipient not connected: " + toUser);
            }
            var sender = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user);
            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.UserName == toUser);
            var chatSession = await _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => (cs.User1Id == sender.Id && cs.User2Id == recipient.Id) ||
                                            (cs.User1Id == recipient.Id && cs.User2Id == sender.Id));

            if (chatSession == null)
            {
                chatSession = new ChatSession
                {
                    User1Id = sender.Id,
                    User2Id = recipient.Id,
                    Messages = new List<Message>()
                };
                _context.ChatSessions.Add(chatSession);
            }

            var newMessage = new Message
            {
                Content = message,
                Timestamp = DateTime.Now,
                SenderId = sender.Id,
                ChatSessionId = chatSession.Id
            };

            chatSession.Messages.Add(newMessage);
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();
        }
    }
}
