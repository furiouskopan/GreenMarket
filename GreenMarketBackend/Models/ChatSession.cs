namespace GreenMarketBackend.Models
{
    public class ChatSession
    {
        public int Id { get; set; }
        public string User1Id { get; set; }
        public ApplicationUser User1 { get; set; }
        public string User2Id { get; set; }
        public ApplicationUser User2 { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
