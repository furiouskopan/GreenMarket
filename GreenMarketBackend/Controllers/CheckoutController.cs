using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels;
using System;
using GreenMarketBackend.Models.ViewModels.CartViewModels;
using GreenMarketBackend.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Identity;
using static GreenMarketBackend.Models.Order;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EmailService _emailService;

    public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, EmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        var cartItems = cart.CartItems.Select(ci => new CartItemViewModel
        {
            CartItemId = ci.CartItemId,
            ProductName = ci.Product.Name,
            Quantity = ci.Quantity,
            Price = ci.Product.Price
        }).ToList();

        var viewModel = new CheckoutViewModel
        {
            Address = string.Empty,
            PaymentMethod = string.Empty,
            CartItems = cartItems,
            TotalAmount = cartItems.Sum(ci => ci.Total)
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessOrder(CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return await RebuildCartView(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        // Create Order
        var order = CreateOrderFromCart(cart, userId, model.Address, model.PaymentMethod);

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cart.CartItems);
        await _context.SaveChangesAsync();

        // Send buyer confirmation email
        await SendOrderConfirmationEmail(order);

        // Send notifications to sellers
        await SendSellerNotificationEmails(order);

        return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
    }

    private Order CreateOrderFromCart(Cart cart, string userId, string shippingAddress, string paymentMethod)
    {
        return new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod,
            Status = OrderStatus.Pending,
            OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                PriceAtTimeOfPurchase = ci.Product.Price
            }).ToList()
        };
    }

    private async Task<IActionResult> RebuildCartView(CheckoutViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        var cartViewModel = new CheckoutViewModel
        {
            Address = model.Address,
            PaymentMethod = model.PaymentMethod,
            CartItems = cart.CartItems.Select(ci => new CartItemViewModel
            {
                CartItemId = ci.CartItemId,
                ProductName = ci.Product.Name,
                Quantity = ci.Quantity,
                Price = ci.Product.Price
            }).ToList(),
            TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price)
        };

        return View("Index", cartViewModel);
    }

    public async Task<IActionResult> OrderConfirmation(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.CreatedByUser)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound();
        }

        var viewModel = new OrderViewModel
        {
            OrderId = order.OrderId,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            PaymentMethod = order.PaymentMethod,
            Status = order.Status,
            OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                Price = oi.PriceAtTimeOfPurchase
            }).ToList()
        };

        return View(viewModel);
    }

    private async Task SendOrderConfirmationEmail(Order order)
    {
        var buyer = await _userManager.FindByIdAsync(order.UserId);
        var sellers = new List<ApplicationUser>();

        // Get unique sellers for the products in the order
        foreach (var orderItem in order.OrderItems)
        {
            var seller = await _userManager.FindByIdAsync(orderItem.Product.CreatedByUserId);
            if (seller != null && !sellers.Contains(seller))
            {
                sellers.Add(seller);
            }
        }

        var subject = $"Order Confirmation - Order #{order.OrderId}";

        // Pass buyerName and sellers' info
        var message = BuildOrderConfirmationEmailBody(order, buyer.UserName, sellers);

        await _emailService.SendEmailAsync(buyer.Email, subject, message);
    }

    private string BuildOrderConfirmationEmailBody(Order order, string buyerName, List<ApplicationUser> sellers)
    {
        var sellerInfo = string.Join("<br/>", sellers.Select(s => $@"
        <p><strong>Seller Name:</strong> {s.UserName}</p>
        <p><strong>Email:</strong> {s.Email}</p>
        <p><strong>Phone:</strong> {s.PhoneNumber ?? "Not provided"}</p>"));

        return $@"
        <h1>Thank you for your order!</h1>
        <p>Hi {buyerName},</p>
        <p>We have received your order and it is currently being processed. Here are your order details:</p>
        <h2>Order #{order.OrderId}</h2>
        <p><strong>Order Date:</strong> {order.OrderDate:f}</p>
        <p><strong>Total Amount:</strong> {order.TotalAmount:C}</p>
        <p><strong>Shipping Address:</strong> {order.ShippingAddress}</p>
        <p><strong>Payment Method:</strong> {order.PaymentMethod}</p>

        <h3>Seller Contact Information:</h3>
        {sellerInfo}

        <h3>Order Items</h3>
        <table border='1' cellpadding='5' cellspacing='0' width='100%'>
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Quantity</th>
                    <th>Price</th>
                    <th>Total</th>
                </tr>
            </thead>
            <tbody>
                {string.Join("", order.OrderItems.Select(item => $@"
                    <tr>
                        <td>{item.Product.Name}</td>
                        <td>{item.Quantity}</td>
                        <td>{item.PriceAtTimeOfPurchase:C}</td>
                        <td>{item.Quantity * item.PriceAtTimeOfPurchase:C}</td>
                    </tr>"))}
            </tbody>
        </table>

        <p>If you have any questions, please contact the seller or our support team.</p>
        <p>Best regards,<br>Your Store Team</p>";
    }

    private async Task SendSellerNotificationEmails(Order order)
    {
        var buyer = await _userManager.FindByIdAsync(order.UserId);

        foreach (var sellerGroup in order.OrderItems
            .Where(oi => !string.IsNullOrEmpty(oi.Product.CreatedByUserId)) // Ensure CreatedByUserId is not null or empty
            .GroupBy(oi => oi.Product.CreatedByUserId))
        {
            var sellerUserId = sellerGroup.Key;
            var seller = await _userManager.FindByIdAsync(sellerUserId);

            if (seller == null)
            {
                Console.WriteLine($"Warning: Seller with ID {sellerUserId} is null for some order items in Order #{order.OrderId}.");
                continue;
            }

            var subject = $"New Order - Items Sold in Order #{order.OrderId}";
            var message = BuildSellerNotificationEmailBody(sellerGroup, order.OrderId, seller.UserName, buyer.UserName, buyer.Email, order.ShippingAddress);

            // Send the email to the seller
            await _emailService.SendEmailAsync(seller.Email, subject, message);
        }
    }

    private string BuildSellerNotificationEmailBody(IGrouping<string, OrderItem> sellerGroup, int orderId, string sellerName, string buyerName, string buyerEmail, string shippingAddress)
    {
        return $@"
            <h1>New Order: #{orderId}</h1>
            <p>Hi {sellerName},</p>
            <p>You have sold the following items:</p>
            <table border='1' cellpadding='5' cellspacing='0' width='100%'>
                <thead>
                    <tr>
                        <th>Product</th>
                        <th>Quantity</th>
                        <th>Price</th>
                        <th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    {string.Join("", sellerGroup.Select(item => $@"
                        <tr>
                            <td>{item.Product.Name}</td>
                            <td>{item.Quantity}</td>
                            <td>{item.PriceAtTimeOfPurchase:C}</td>
                            <td>{item.Quantity * item.PriceAtTimeOfPurchase:C}</td>
                        </tr>"))}
                </tbody>
            </table>

            <h3>Buyer Contact Information</h3>
            <p><strong>Buyer Name:</strong> {buyerName}</p>
            <p><strong>Email:</strong> {buyerEmail}</p>
            <p><strong>Shipping Address:</strong> {shippingAddress}</p>

            <p>Please arrange the delivery of these items with the buyer. If you have any questions, feel free to contact us.</p>
            <p>Best regards,<br>Your Store Team</p>";
    }
}