namespace GreenMarketBackend.Models.ViewModels.CartViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }

        public CartViewModel()
        {
            CartItems = new List<CartItem>();
        }
    }
}
