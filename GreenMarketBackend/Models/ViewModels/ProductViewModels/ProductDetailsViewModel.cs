namespace GreenMarketBackend.Models.ViewModels.ProductViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } = new Product();
        public int ProductId { get; set; }
        public List<Review> Reviews { get; set; }
        public ReviewSubmissionViewModel NewReview { get; set; }
        public List<ProductImage> Images { get; set; }
        public string MainImageUrl { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public ApplicationUser Uploader { get; set; }
        public bool IsFeatured { get; set; }
    }
}