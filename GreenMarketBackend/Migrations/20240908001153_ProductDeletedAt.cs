using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class ProductDeletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "e60b3062-bed5-4a39-82c2-e3147f731da4", "AQAAAAIAAYagAAAAENxma+mwHOX6aa8qoYBk/DkRVU8WjCc6lthrsePeSn5Q6IXIKzvAhge+W9p0M+BvoQ==", new DateTime(2024, 9, 8, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6082), "99d5b5e9-48fc-48b8-9465-704f854bc3f0" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DeletedAt", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 8, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6375), null, new DateTime(2024, 8, 9, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DeletedAt", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 8, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6385), null, new DateTime(2024, 8, 29, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6387) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "22daeb04-26bf-4fdd-bd69-34a2e797a432", "AQAAAAIAAYagAAAAEJwkvxcLqC4ajiVodTJYaXiK+t1HrwIlBWB61DMfnqC5/F2IhKQ/oGqQ4tne6JUwHw==", new DateTime(2024, 9, 6, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(6933), "ff282cd6-e30d-43ef-adbe-1bac0c9c3327" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 6, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7249), new DateTime(2024, 8, 7, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7254) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 6, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7260), new DateTime(2024, 8, 27, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7262) });
        }
    }
}
