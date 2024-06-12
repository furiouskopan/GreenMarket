using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChatNesto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Messages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ChatSessionId",
                table: "ChatSessions",
                newName: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Messages",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ChatSessions",
                newName: "ChatSessionId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "554a0085-2e80-40b2-9cda-ffe87189b42d", "AQAAAAIAAYagAAAAEDZkQgFF5F5wy5NjbA8K/I7FOUieSQoP74HF59RdOJXlNS6UocGI1ou1UroAnFyVpw==", new DateTime(2024, 6, 10, 17, 16, 54, 582, DateTimeKind.Local).AddTicks(4958), "788a714c-e12a-4e60-b16e-630216ad5a89" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 10, 17, 16, 54, 582, DateTimeKind.Local).AddTicks(5271), new DateTime(2024, 5, 11, 17, 16, 54, 582, DateTimeKind.Local).AddTicks(5275) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 10, 17, 16, 54, 582, DateTimeKind.Local).AddTicks(5282), new DateTime(2024, 5, 31, 17, 16, 54, 582, DateTimeKind.Local).AddTicks(5284) });
        }
    }
}
