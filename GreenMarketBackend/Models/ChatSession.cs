namespace GreenMarketBackend.Models
{
    public class ChatSession
    {
        public int ChatSessionId { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public virtual ApplicationUser User1 { get; set; }
        public virtual ApplicationUser User2 { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
