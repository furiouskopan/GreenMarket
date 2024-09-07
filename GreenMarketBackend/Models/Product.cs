using System;
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

        [Required]
        public string ImageURL { get; set; }
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

        //Navigation property to represent the user who created the product
        public virtual ApplicationUser CreatedByUser { get; set; }
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        // Parental data for hierarchical categories
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}