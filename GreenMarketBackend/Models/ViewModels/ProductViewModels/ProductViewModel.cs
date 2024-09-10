using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
namespace GreenMarketBackend.Models.ViewModels.ProductViewModels
{
    public class ProductViewModel
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

        [Required]
        public int MainImageIndex { get; set; }

        [Required]
        public string Pesticides { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public DateTime HarvestDate { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}