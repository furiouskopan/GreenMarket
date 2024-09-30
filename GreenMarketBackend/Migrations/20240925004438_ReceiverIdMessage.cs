using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class ReceiverIdMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "28bb962b-2eb9-4ae1-a5cf-dc16f029356c", "AQAAAAIAAYagAAAAENIKP3J7yPlz+GTHGBXg5rzqPGo1hiGNwR+HTdaQSr3Cq+5EeX86adgbzjVsPlLkbg==", new DateTime(2024, 9, 25, 2, 44, 36, 899, DateTimeKind.Local).AddTicks(7019), "081682c3-50ac-4509-bd3c-d623cb3b4f02" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 123,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 25, 2, 44, 36, 899, DateTimeKind.Local).AddTicks(7323), new DateTime(2024, 8, 26, 2, 44, 36, 899, DateTimeKind.Local).AddTicks(7326) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "87d67e93-f9a7-4918-833e-92bff5f2d7ad", "AQAAAAIAAYagAAAAEM4RDSEWR8OFP+ngyl10amXAeab6Amh0U61Y4PWuScnBUPQsW50zIxUtEVScyiFN7Q==", new DateTime(2024, 9, 24, 17, 47, 4, 141, DateTimeKind.Local).AddTicks(1174), "bdd2fcc3-0e21-421d-82df-2c8acf91abed" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 123,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 24, 17, 47, 4, 141, DateTimeKind.Local).AddTicks(1492), new DateTime(2024, 8, 25, 17, 47, 4, 141, DateTimeKind.Local).AddTicks(1499) });
        }
    }
}
