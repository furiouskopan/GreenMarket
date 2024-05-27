using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class ReviewNesto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "b7e29788-15b3-43bf-a4f9-7f34b09fa37c", "AQAAAAIAAYagAAAAEBCoxu374NN4WISnk4uS8BYBA29MaaQVx4PNa9rMZWJOJzugXgDsPhxewMGiw8Bxfw==", new DateTime(2024, 5, 27, 20, 22, 46, 241, DateTimeKind.Local).AddTicks(3629), "6460a4bb-8ea2-459a-9675-dd283eaf55f9" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 5, 27, 20, 22, 46, 241, DateTimeKind.Local).AddTicks(3907), new DateTime(2024, 4, 27, 20, 22, 46, 241, DateTimeKind.Local).AddTicks(3910) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 5, 27, 20, 22, 46, 241, DateTimeKind.Local).AddTicks(3975), new DateTime(2024, 5, 17, 20, 22, 46, 241, DateTimeKind.Local).AddTicks(3977) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "5e58b356-7702-4296-98cf-6e26476704ad", "AQAAAAIAAYagAAAAEHap0XyMTtoDyKX0RPji48JcZlBimQfdvLkH7cQrngWOlcf2TvUf7+4iRWKryQYL9w==", new DateTime(2024, 5, 21, 22, 52, 29, 766, DateTimeKind.Local).AddTicks(8991), "34f3f4fa-38d6-4677-8d33-a5abb3a3575e" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 5, 21, 22, 52, 29, 766, DateTimeKind.Local).AddTicks(9400), new DateTime(2024, 4, 21, 22, 52, 29, 766, DateTimeKind.Local).AddTicks(9763) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 5, 21, 22, 52, 29, 766, DateTimeKind.Local).AddTicks(9795), new DateTime(2024, 5, 11, 22, 52, 29, 766, DateTimeKind.Local).AddTicks(9799) });
        }
    }
}
