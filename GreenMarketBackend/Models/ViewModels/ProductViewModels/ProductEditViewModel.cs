using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models.ViewModels.ProductViewModels
{
    public class ProductEditViewModel
    {
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
        public int MainImageIndex { get; set; }

        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
        public List<ProductImage> ExistingImages { get; set; } = new List<ProductImage>();

        public bool IsRemovingExistingImage { get; set; }

        [Required]
        public string Pesticides { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public DateTime HarvestDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
