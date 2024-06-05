using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public DateTime DateCreated { get; set; }

        public decimal TotalAmount { get; set; }

        // Navigation property for cart items
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
