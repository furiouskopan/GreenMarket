using Microsoft.AspNetCore.SignalR;

namespace GreenMarketBackend.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int chatSessionId, string senderId, string content)
        {
            await Clients.All.SendAsync("ReceiveMessage", chatSessionId, senderId, content);
        }
    }
}
