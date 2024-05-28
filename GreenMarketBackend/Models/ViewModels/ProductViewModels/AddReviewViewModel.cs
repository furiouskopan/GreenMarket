using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models.ViewModels.ProductViewModels
{
    public class AddReviewViewModel
    {
        public int ProductId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
