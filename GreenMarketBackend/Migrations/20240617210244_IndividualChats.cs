using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class IndividualChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "59266dd9-934b-4ecf-bb47-84ab530d2d1c", "AQAAAAIAAYagAAAAEHo9ie7973K4kIrOWgp69knUzOdHdDlsisnBof8X+GylbZlPgBrSxAt+CWARqa9B1Q==", new DateTime(2024, 6, 12, 14, 15, 12, 539, DateTimeKind.Local).AddTicks(5080), "06a06205-fed7-48c0-91bc-53347cf2d331" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 12, 14, 15, 12, 539, DateTimeKind.Local).AddTicks(5385), new DateTime(2024, 5, 13, 14, 15, 12, 539, DateTimeKind.Local).AddTicks(5388) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 12, 14, 15, 12, 539, DateTimeKind.Local).AddTicks(5396), new DateTime(2024, 6, 2, 14, 15, 12, 539, DateTimeKind.Local).AddTicks(5398) });
        }
    }
}
