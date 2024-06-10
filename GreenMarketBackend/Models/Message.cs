namespace GreenMarketBackend.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ChatSessionId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual ChatSession ChatSession { get; set; }
        public virtual ApplicationUser Sender { get; set; } 
    }
}
