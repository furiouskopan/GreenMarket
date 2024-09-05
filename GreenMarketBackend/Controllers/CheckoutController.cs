using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using GreenMarketBackend.Models.ViewModels;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using GreenMarketBackend.Models.ViewModels.CartViewModels;
using GreenMarketBackend.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Identity;
using static GreenMarketBackend.Models.Order;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
        if (ModelState.IsValid)
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

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
                ShippingAddress = model.Address,
                PaymentMethod = model.PaymentMethod,
                Status = OrderStatus.Pending,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    PriceAtTimeOfPurchase = ci.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            // Send email confirmation
            await SendOrderConfirmationEmail(order);

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        var userCart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

        var cartViewModel = new CheckoutViewModel
        {
            Address = model.Address,
            PaymentMethod = model.PaymentMethod,
            CartItems = userCart.CartItems.Select(ci => new CartItemViewModel
            {
                CartItemId = ci.CartItemId,
                ProductName = ci.Product.Name,
                Quantity = ci.Quantity,
                Price = ci.Product.Price
            }).ToList(),
            TotalAmount = userCart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price)
        };

        return View("Index", cartViewModel);
    }

    public async Task<IActionResult> OrderConfirmation(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
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
        var user = await _userManager.FindByIdAsync(order.UserId);
        var email = user.Email;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your Store", "no-reply@yourstore.com"));
        message.To.Add(new MailboxAddress(user.UserName, email));
        message.Subject = "Order Confirmation - Order #" + order.OrderId;

        // Create the HTML body of the email
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
        <h1>Thank you for your order!</h1>
        <p>Hi {user.UserName},</p>
        <p>We have received your order and it is currently being processed. Here are your order details:</p>
        <h2>Order #{order.OrderId}</h2>
        <p><strong>Order Date:</strong> {order.OrderDate:f}</p>
        <p><strong>Total Amount:</strong> {order.TotalAmount:C}</p>
        <p><strong>Shipping Address:</strong> {order.ShippingAddress}</p>
        <p><strong>Payment Method:</strong> {order.PaymentMethod}</p>
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
        <p>If you have any questions, please contact our support team.</p>
        <p>Best regards,<br>Your Store Team</p>"
        };

        // Create the plain text body as a fallback
        bodyBuilder.TextBody = $@"
    Thank you for your order!

    Hi {user.UserName},

    We have received your order and it is currently being processed. Here are your order details:

    Order #{order.OrderId}
    Order Date: {order.OrderDate:f}
    Total Amount: {order.TotalAmount:C}
    Shipping Address: {order.ShippingAddress}
    Payment Method: {order.PaymentMethod}

    Order Items:
    {string.Join("\n", order.OrderItems.Select(item => $@"
        Product: {item.Product.Name}
        Quantity: {item.Quantity}
        Price: {item.PriceAtTimeOfPurchase:C}
        Total: {(item.Quantity * item.PriceAtTimeOfPurchase):C}
    "))}

    If you have any questions, please contact our support team.

    Best regards,
    Your Store Team";

        message.Body = bodyBuilder.ToMessageBody();

        await SendEmailAsync(message);
    }

    private async Task SendSellerNotificationEmails(Order order)
    {
        foreach (var sellerGroup in order.OrderItems.GroupBy(oi => oi.Product.CreatedByUser))
        {
            var seller = sellerGroup.Key;
            var sellerEmail = seller.Email;
            var items = sellerGroup.ToList();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Green Market", "no-reply@greenmarket.com"));
            message.To.Add(new MailboxAddress(seller.UserName, sellerEmail));
            message.Subject = $"New Order - Items Sold in Order #{order.OrderId}";

            // HTML Body for email
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <h1>New Order: #{order.OrderId}</h1>
            <p>Hi {seller.UserName},</p>
            <p>You have sold the following items:</p>
            <table border='1'>
                <thead>
                    <tr>
                        <th>Product</th>
                        <th>Quantity</th>
                        <th>Price</th>
                        <th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    {string.Join("", items.Select(item => $@"
                    <tr>
                        <td>{item.Product.Name}</td>
                        <td>{item.Quantity}</td>
                        <td>{item.PriceAtTimeOfPurchase:C}</td>
                        <td>{item.Quantity * item.PriceAtTimeOfPurchase:C}</td>
                    </tr>"))}
                </tbody>
            </table>
            <p>Please arrange the delivery of these items with the buyer.</p>"
            };

            bodyBuilder.TextBody = $@"
        New Order: #{order.OrderId}
        Hi {seller.UserName},

        You have sold the following items:

        {string.Join("\n", items.Select(item => $@"
            Product: {item.Product.Name}
            Quantity: {item.Quantity}
            Price: {item.PriceAtTimeOfPurchase:C}
            Total: {item.Quantity * item.PriceAtTimeOfPurchase:C}
        "))}

        Please arrange the delivery of these items with the buyer.";

            message.Body = bodyBuilder.ToMessageBody();

            await SendEmailAsync(message);
        }
    }

    // This is the common method to send email
    private async Task SendEmailAsync(MimeMessage message)
    {
        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(Environment.GetEnvironmentVariable("SMTP_USER"), Environment.GetEnvironmentVariable("SMTP_PASS"));
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Failed to send email: {ex.Message}");
            // Consider logging with a proper logging framework here
        }
    }
}