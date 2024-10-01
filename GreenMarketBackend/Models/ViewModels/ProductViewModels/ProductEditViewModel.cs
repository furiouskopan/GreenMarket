using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        // Existing images' URLs
        public List<string> ExistingImageUrls { get; set; } = new List<string>();

        // New images for upload
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

        // URL of the selected main image
        [Required]
        public string MainImageUrl { get; set; }

        [Required]
        public string Pesticides { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public DateTime HarvestDate { get; set; }

        [Required]
        public int? ParentCategoryId { get; set; }
        [Required]
        public int? ChildCategoryId { get; set; }

        public IEnumerable<SelectListItem> ParentCategories { get; set; }
        public IEnumerable<SelectListItem> ChildCategories { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsRemovingExistingImage { get; set; } // For handling image removals
    }

}
