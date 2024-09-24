using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReadToMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "0dc60e3e-1c7d-4f7f-ad47-6fc1db6ecb95", "AQAAAAIAAYagAAAAEMzCNwnS0EB97BS/utodLhG/ff9edrjQjeL5a07DiPb5uz2dYwJ4H4WkcsEboggczQ==", new DateTime(2024, 9, 17, 3, 7, 16, 775, DateTimeKind.Local).AddTicks(806), "c40289ab-9a2a-427c-9f2a-746a3d422cf8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 123,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 17, 3, 7, 16, 775, DateTimeKind.Local).AddTicks(1070), new DateTime(2024, 8, 18, 3, 7, 16, 775, DateTimeKind.Local).AddTicks(1074) });
        }
    }
}
