using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class IsFeaturedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                columns: new[] { "CreatedDate", "HarvestDate", "IsFeatured" },
                values: new object[] { new DateTime(2024, 9, 17, 3, 7, 16, 775, DateTimeKind.Local).AddTicks(1070), new DateTime(2024, 8, 18, 3, 7, 16, 775, DateTimeKind.Local).AddTicks(1074), false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "4ac95808-59cc-4f1b-8184-f0d5b783c230", "AQAAAAIAAYagAAAAELFKElWBXCazvXnEZLs8jEPLbPTqU1SyQMgXvTcEDD6NogMl/zc8aOpS33jFrZ80SQ==", new DateTime(2024, 9, 12, 2, 18, 1, 968, DateTimeKind.Local).AddTicks(572), "9bed910e-db81-40f9-a53a-d2962fb4fccb" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 123,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 12, 2, 18, 1, 968, DateTimeKind.Local).AddTicks(920), new DateTime(2024, 8, 13, 2, 18, 1, 968, DateTimeKind.Local).AddTicks(923) });
        }
    }
}
