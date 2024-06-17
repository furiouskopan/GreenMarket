namespace GreenMarketBackend.Models.ViewModels.ChatViewModels
{
    public class ChatViewModel
    {
        public List<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
        public int SelectedChatSessionId { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
