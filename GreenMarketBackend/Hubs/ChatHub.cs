using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GreenMarketBackend.Hubs
{
    public class ChatHub : Hub
    {
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
        }
    }
}
