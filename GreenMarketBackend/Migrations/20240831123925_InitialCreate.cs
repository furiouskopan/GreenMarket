using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "22acc6e0-f7c4-4c9d-ac78-1a66c883c3da", "AQAAAAIAAYagAAAAECpFdMDyI9qkUw2HMRCynjweZklGjQrBrRs+WmNaxiaFJ7+m6+mC+CuOcoxfK5+aAA==", new DateTime(2024, 6, 17, 23, 2, 43, 13, DateTimeKind.Local).AddTicks(7045), "a3281622-09f6-446e-86a5-0cc1a2a22f3c" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 17, 23, 2, 43, 13, DateTimeKind.Local).AddTicks(7337), new DateTime(2024, 5, 18, 23, 2, 43, 13, DateTimeKind.Local).AddTicks(7340) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 17, 23, 2, 43, 13, DateTimeKind.Local).AddTicks(7348), new DateTime(2024, 6, 7, 23, 2, 43, 13, DateTimeKind.Local).AddTicks(7350) });
        }
    }
}
