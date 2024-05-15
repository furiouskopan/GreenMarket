using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsSeller", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a1234567-89ab-cdef-0123-456789abcdef", 0, "Epimenonda", "f59d3720-3f37-4f60-ac4d-7c867dc66e99", "testuser@example.com", true, "Test", true, "User", false, null, "TESTUSER@EXAMPLE.COM", "TESTUSER", "AQAAAAIAAYagAAAAECK6j0LutiQcFnFtNqbYSzpEmi6SQ10Bj9gxsg2rQ388AFfRA3U2bdaA7xyK3oQpFQ==", null, false, new DateTime(2024, 5, 15, 22, 19, 0, 248, DateTimeKind.Local).AddTicks(7356), "aff265ba-b72b-4b30-b752-bb6c6942fd04", false, "testuser" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AverageRating", "CategoryId", "CreatedByUserId", "CreatedDate", "Description", "HarvestDate", "ImageURL", "Name", "Origin", "ParentCategoryId", "Pesticides", "Price", "ReviewCount", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 4.5, 1, "a1234567-89ab-cdef-0123-456789abcdef", new DateTime(2024, 5, 15, 22, 19, 0, 248, DateTimeKind.Local).AddTicks(7638), "Fresh apples from local orchards.", new DateTime(2024, 4, 15, 22, 19, 0, 248, DateTimeKind.Local).AddTicks(7642), "path_to_apples.jpg", "Organic Apples", "Local", null, "None", 1.99m, 10, 100 },
                    { 2, 4.7999999999999998, 2, "a1234567-89ab-cdef-0123-456789abcdef", new DateTime(2024, 5, 15, 22, 19, 0, 248, DateTimeKind.Local).AddTicks(7648), "Crunchy carrots perfect for a healthy snack.", new DateTime(2024, 5, 5, 22, 19, 0, 248, DateTimeKind.Local).AddTicks(7650), "path_to_carrots.jpg", "Organic Carrots", "Local", null, "None", 0.99m, 8, 150 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef");
        }
    }
}
