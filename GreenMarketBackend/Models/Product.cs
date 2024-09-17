using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenMarketBackend.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        // Images associated with the product
        [Required]
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        // Index of the main image to display
        public int MainImageIndex { get; set; } = 0; // Default to the first image

        [Required]
        public string Pesticides { get; set; }

        [Required]
        public string Origin { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime HarvestDate { get; set; }

        [ForeignKey("User")]
        public string CreatedByUserId { get; set; }

        public virtual ApplicationUser CreatedByUser { get; set; }

        public DateTime? DeletedAt { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public ICollection<OrderItem> OrderItens { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }
        public bool IsFeatured { get; set; }
    }
}
