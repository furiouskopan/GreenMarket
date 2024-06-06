using GreenMarketBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public Cart Cart { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
    }
}
