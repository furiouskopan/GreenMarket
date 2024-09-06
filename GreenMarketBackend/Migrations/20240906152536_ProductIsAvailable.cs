using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class ProductIsAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                columns: new[] { "CreatedDate", "HarvestDate", "IsAvailable" },
                values: new object[] { new DateTime(2024, 9, 6, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7249), new DateTime(2024, 8, 7, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7254), false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate", "IsAvailable" },
                values: new object[] { new DateTime(2024, 9, 6, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7260), new DateTime(2024, 8, 27, 17, 25, 35, 377, DateTimeKind.Local).AddTicks(7262), false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "fa60db48-49c6-460d-a9b6-38d50ff5feda", "AQAAAAIAAYagAAAAEDXYIGIWpgbFCNM/vekihOHvoZhs2L/tOSCCpykSBKeuxktDZDkAGI1iFVZbRPQBkw==", new DateTime(2024, 8, 31, 14, 39, 23, 579, DateTimeKind.Local).AddTicks(9737), "3c3d6536-03c7-4191-bade-e0bb130a0ab1" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 8, 31, 14, 39, 23, 580, DateTimeKind.Local).AddTicks(29), new DateTime(2024, 8, 1, 14, 39, 23, 580, DateTimeKind.Local).AddTicks(33) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 8, 31, 14, 39, 23, 580, DateTimeKind.Local).AddTicks(41), new DateTime(2024, 8, 21, 14, 39, 23, 580, DateTimeKind.Local).AddTicks(43) });
        }
    }
}
