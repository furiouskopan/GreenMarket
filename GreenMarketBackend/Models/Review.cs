using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [Range(1, 5)] // Assuming a 1-5 star rating system
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public string UserId { get; set; } // Foreign Key for the user who wrote the review
        public virtual ApplicationUser User { get; set; } // Navigation property

        public int ProductId { get; set; } // Foreign Key
        public virtual Product Product { get; set; } // Navigation property

        public DateTime CreatedDate { get; set; }
    }
}
