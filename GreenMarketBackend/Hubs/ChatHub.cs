using Microsoft.AspNetCore.SignalR;

namespace GreenMarketBackend.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw; // Re-throwing the exception ensures that the client can see there was a failure
            }
        }
    }

}
