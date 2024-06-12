using Microsoft.AspNetCore.SignalR;

namespace GreenMarketBackend.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string sender, string recipient, string message)
        {
            var recipientConnectionId = ConnectionMapping.GetConnectionId(recipient);
            if (recipientConnectionId != null)
            {
                await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", sender, message);
            }
            else
            {
                await Clients.Caller.SendAsync("UserNotFound", recipient);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            ConnectionMapping.Add(userName, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.Identity.Name;
            ConnectionMapping.Remove(userName);
            await base.OnDisconnectedAsync(exception);
        }
    }

    public static class ConnectionMapping
    {
        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public static void Add(string userName, string connectionId)
        {
            _connections[userName] = connectionId;
        }

        public static void Remove(string userName)
        {
            _connections.Remove(userName);
        }

        public static string GetConnectionId(string userName)
        {
            return _connections.TryGetValue(userName, out var connectionId) ? connectionId : null;
        }
    }
}
