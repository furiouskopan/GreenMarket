namespace GreenMarketBackend.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatSessionId { get; set; }
        public ChatSession ChatSession { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
