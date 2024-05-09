using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string Category { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    [Required]
    public string ImageURL { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("User")]
    public string CreatedByUserId { get; set; }

    // Navigation property to represent the user who created the product
    //public virtual ApplicationUser CreatedByUser { get; set; }
}
