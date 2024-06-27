using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using GreenMarketBackend.Data;
using GreenMarketBackend.Models;
using GreenMarketBackend.ViewModels;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using GreenMarketBackend.Models.ViewModels.CartViewModels;
using GreenMarketBackend.Models.ViewModels.OrderViewModels;
using GreenMarketBackend.Models.ViewModels;
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
        message.Subject = "Order Confirmation";

        message.Body = new TextPart("plain")
        {
            Text = $"Thank you for your order! Your order ID is {order.OrderId}."
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.your-email-provider.com", 587, false);
            await client.AuthenticateAsync("your-email", "your-email-password");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
