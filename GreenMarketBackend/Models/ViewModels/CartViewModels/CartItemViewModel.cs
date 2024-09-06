namespace GreenMarketBackend.Models.ViewModels.CartViewModels
{
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
        public bool IsAvailable { get; set; }
    }
}
