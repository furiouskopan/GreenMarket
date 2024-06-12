namespace GreenMarketBackend.Models.ViewModels.ChatViewModels
{
    public class ChatViewModel
    {
        public int ChatSessionId {  get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
