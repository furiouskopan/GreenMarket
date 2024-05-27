using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models.ViewModels
{
    public class ReviewSubmissionViewModel
    {
        [Required(ErrorMessage = "The ProductId field is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "The Comment field is required.")]
        [StringLength(500, ErrorMessage = "The Comment cannot exceed 500 characters.")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "The Rating field is required.")]
        [Range(1, 5, ErrorMessage = "The Rating must be between 1 and 5.")]
        public int Rating { get; set; }
    }
}
