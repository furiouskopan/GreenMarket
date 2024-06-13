namespace GreenMarketBackend.Models
{
    public class MessageModel
    {
        public int ChatSessionId { get; set; }
        public string Content { get; set; }
        public string ToUser { get; set; }
    }
}
