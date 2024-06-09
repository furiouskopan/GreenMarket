using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels.CartViewModels;
using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Shipping Address is required")]
        [Display(Name = "Shipping Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Payment Method is required")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        // Properties to display in the view
        public IEnumerable<CartItemViewModel> CartItems { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
