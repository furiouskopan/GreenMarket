using GreenMarketBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GreenMarketBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("AspNetUsers"); // Renaming the ASP.NET Identity table name to 'AspNetUsers'
            });

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReporterUser)
                .WithMany()
                .HasForeignKey(r => r.ReporterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedUser)
                .WithMany()
                .HasForeignKey(r => r.ReportedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the navigation properties for hierarchical categories
            modelBuilder.Entity<Category>()
                .HasMany(c => c.ChildCategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            // Configure indexes
            modelBuilder.Entity<Product>().HasIndex(p => p.Name);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seeding categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Fruits", Description = "All kinds of fruits", ParentCategoryId = null },
                new Category { CategoryId = 2, Name = "Vegetables", Description = "Fresh vegetables", ParentCategoryId = null },
                new Category { CategoryId = 3, Name = "Citrus Fruits", Description = "All kinds of citrus fruits", ParentCategoryId = 1 },
                new Category { CategoryId = 4, Name = "Root Vegetables", Description = "Various root vegetables", ParentCategoryId = 2 }
            );


            // Seed a dummy user
            var userId = "a1234567-89ab-cdef-0123-456789abcdef"; // Ensure this is unique and consistent
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                NormalizedUserName = "TESTUSER",
                Email = "testuser@example.com",
                NormalizedEmail = "TESTUSER@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Test123!"), // Use a password hasher
                SecurityStamp = Guid.NewGuid().ToString(),  // Generating a new security stamp
                FirstName = "Test",
                LastName = "User",
                Address = "Epimenonda",
                RegistrationDate = DateTime.Now,
                IsSeller = true
            });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Organic Apples",
                    Description = "Fresh apples from local orchards.",
                    Price = 1.99m,
                    StockQuantity = 100,
                    ImageURL = "path_to_apples.jpg",
                    Pesticides = "None",
                    Origin = "Local",
                    CreatedDate = DateTime.Now,
                    HarvestDate = DateTime.Now.AddDays(-30),
                    CreatedByUserId = userId,  // Ensure this ID exists in your User table
                    CategoryId = 1,  // Ensure this ID exists in your Category table
                    AverageRating = 4.5,
                    ReviewCount = 10
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Organic Carrots",
                    Description = "Crunchy carrots perfect for a healthy snack.",
                    Price = 0.99m,
                    StockQuantity = 150,
                    ImageURL = "path_to_carrots.jpg",
                    Pesticides = "None",
                    Origin = "Local",
                    CreatedDate = DateTime.Now,
                    HarvestDate = DateTime.Now.AddDays(-10),
                    CreatedByUserId = userId,  // Same note as above
                    CategoryId = 2,  // Same note as above
                    AverageRating = 4.8,
                    ReviewCount = 8
                }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-S5ULU19\\SQLEXPRESS; Database=GreenMarket; Trusted_Connection=True; TrustServerCertificate=True");
            }
        }
    }
}
