using static GreenMarketBackend.Models.Order;

namespace GreenMarketBackend.Models.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
