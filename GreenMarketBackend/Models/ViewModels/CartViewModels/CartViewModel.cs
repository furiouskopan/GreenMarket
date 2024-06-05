namespace GreenMarketBackend.Models.ViewModels.CartViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal TotalAmount { get; set; }
    }
}
