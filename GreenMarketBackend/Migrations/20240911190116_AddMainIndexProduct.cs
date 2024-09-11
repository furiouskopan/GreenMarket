using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddMainIndexProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_ParentCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ParentCategoryId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "MainImageIndex",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "cbeef5dc-87df-494c-9834-7c87cb931974", "AQAAAAIAAYagAAAAEL04IUBZ2H7oU0tlx4GZT4fh6kW+boD6+UwzSQYkuyRuAl3547sbd7UsidqVh8dfKA==", new DateTime(2024, 9, 11, 21, 1, 15, 241, DateTimeKind.Local).AddTicks(9879), "0b257dab-9cd5-44e5-aa2b-9f4ed8ed70d3" });

            migrationBuilder.InsertData(
                table: "ProductImage",
                columns: new[] { "Id", "ImageUrl", "IsMain", "ProductId" },
                values: new object[] { 1, "path_to_apples.jpg", false, 12 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AverageRating", "CategoryId", "CreatedByUserId", "CreatedDate", "DeletedAt", "Description", "HarvestDate", "IsAvailable", "MainImageIndex", "Name", "Origin", "Pesticides", "Price", "ReviewCount", "StockQuantity" },
                values: new object[] { 123, 4.5, 1, "a1234567-89ab-cdef-0123-456789abcdef", new DateTime(2024, 9, 11, 21, 1, 15, 242, DateTimeKind.Local).AddTicks(205), null, "Fresh apples from local orchards.", new DateTime(2024, 8, 12, 21, 1, 15, 242, DateTimeKind.Local).AddTicks(208), false, 0, "Organic Apples", "Local", "None", 1.99m, 10, 100 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImage",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 123);

            migrationBuilder.DropColumn(
                name: "MainImageIndex",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "faa7b9a4-8ff5-49ac-8f0d-8e456dd99293", "AQAAAAIAAYagAAAAEJSgUAhfGyr5hSPO8A0EoKvPOzRLW1nOseD8oMXlwCGNd9ZldxdBP4zPWfIxig5qxA==", new DateTime(2024, 9, 10, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7500), "732dc6ba-75a7-4695-8672-f061b1dfe6e8" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AverageRating", "CategoryId", "CreatedByUserId", "CreatedDate", "DeletedAt", "Description", "HarvestDate", "ImageURL", "IsAvailable", "Name", "Origin", "ParentCategoryId", "Pesticides", "Price", "ReviewCount", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 4.5, 1, "a1234567-89ab-cdef-0123-456789abcdef", new DateTime(2024, 9, 10, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7805), null, "Fresh apples from local orchards.", new DateTime(2024, 8, 11, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7810), "path_to_apples.jpg", false, "Organic Apples", "Local", null, "None", 1.99m, 10, 100 },
                    { 2, 4.7999999999999998, 2, "a1234567-89ab-cdef-0123-456789abcdef", new DateTime(2024, 9, 10, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7817), null, "Crunchy carrots perfect for a healthy snack.", new DateTime(2024, 8, 31, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7819), "path_to_carrots.jpg", false, "Organic Carrots", "Local", null, "None", 0.99m, 8, 150 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ParentCategoryId",
                table: "Products",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_ParentCategoryId",
                table: "Products",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }
    }
}
