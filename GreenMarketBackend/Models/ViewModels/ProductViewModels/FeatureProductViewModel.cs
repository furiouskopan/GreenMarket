namespace GreenMarketBackend.Models.ViewModels.ProductViewModels
{
    public class FeatureProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string MainImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsFeatured { get; set; }

        // Add these properties if needed
        public string Description { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}
