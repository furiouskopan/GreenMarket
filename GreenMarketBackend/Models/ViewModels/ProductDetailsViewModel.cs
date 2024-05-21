namespace GreenMarketBackend.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }
        public Review NewReview { get; set; }
        public ApplicationUser Uploader { get; set; }
    }
}
