using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class ProductCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "31ddb3d2-7aeb-463e-a3f3-cd08ba150c07", "AQAAAAIAAYagAAAAEEvccRR+S7qGXPVcDrUCg/TXhbThQToFT9muQY59G2FIAh+IdT+tj6lwqMq/Hth/wQ==", new DateTime(2024, 9, 8, 2, 29, 53, 16, DateTimeKind.Local).AddTicks(3964), "92f0de0e-6d15-4909-b229-f6b186d7a163" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 8, 2, 29, 53, 16, DateTimeKind.Local).AddTicks(4290), new DateTime(2024, 8, 9, 2, 29, 53, 16, DateTimeKind.Local).AddTicks(4294) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 8, 2, 29, 53, 16, DateTimeKind.Local).AddTicks(4300), new DateTime(2024, 8, 29, 2, 29, 53, 16, DateTimeKind.Local).AddTicks(4302) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 8, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6375), new DateTime(2024, 8, 9, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6378) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 8, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6385), new DateTime(2024, 8, 29, 2, 11, 52, 281, DateTimeKind.Local).AddTicks(6387) });
        }
    }
}
