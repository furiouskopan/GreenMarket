﻿using GreenMarketBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GreenMarketBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

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
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Restrict deletion in the database

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItens)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Similarly for OrderItems or other related tables


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

            modelBuilder.Entity<ChatSession>()
                .HasOne(cs => cs.User1)
                .WithMany()
                .HasForeignKey(cs => cs.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatSession>()
                .HasOne(cs => cs.User2)
                .WithMany()
                .HasForeignKey(cs => cs.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ChatSession)
                .WithMany(cs => cs.Messages)
                .HasForeignKey(m => m.ChatSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
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
                    ProductId = 123,
                    Name = "Organic Apples",
                    Description = "Fresh apples from local orchards.",
                    Price = 1.99m,
                    StockQuantity = 100,
                    Pesticides = "None",
                    Origin = "Local",
                    CreatedDate = DateTime.Now,
                    HarvestDate = DateTime.Now.AddDays(-30),
                    CreatedByUserId = userId,  // Ensure this ID exists in your User table
                    CategoryId = 1,  // Ensure this ID exists in your Category table
                    AverageRating = 4.5,
                    ReviewCount = 10
                }
                //new Product
                //{
                //    ProductId = 2,
                //    Name = "Organic Carrots",
                //    Description = "Crunchy carrots perfect for a healthy snack.",
                //    Price = 0.99m,
                //    StockQuantity = 150,
                //    Pesticides = "None",
                //    Origin = "Local",
                //    CreatedDate = DateTime.Now,
                //    HarvestDate = DateTime.Now.AddDays(-10),
                //    CreatedByUserId = userId,  // Ensure this ID exists in your User table
                //    CategoryId = 2,  // Ensure this ID exists in your Category table
                //    AverageRating = 4.8,
                //    ReviewCount = 8
                //}
            );

            modelBuilder.Entity<ProductImage>().HasData(
                // Images for Product 1 (Apples)
                new ProductImage
                {
                    Id = 1,
                    ProductId = 12, // Foreign key to Product 1 (Apples)
                    ImageUrl = "path_to_apples.jpg",
                    IsMain = false
                }
            );

            //    // Images for Product 2 (Carrots)
            //    new ProductImage
            //    {
            //        Id = 4,
            //        ProductId = 2, // Foreign key to Product 2 (Carrots)
            //        ImageUrl = "path_to_carrots.jpg",
            //        IsMain = true
            //    },
            //    new ProductImage
            //    {
            //        Id = 5,
            //        ProductId = 2, // Foreign key to Product 2 (Carrots)
            //        ImageUrl = "path_to_carrots2.jpg",
            //        IsMain = false
            //    },
            //    new ProductImage
            //    {
            //        Id = 6,
            //        ProductId = 2, // Foreign key to Product 2 (Carrots)
            //        ImageUrl = "path_to_carrots3.jpg",
            //        IsMain = false
            //    }
            //);
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
