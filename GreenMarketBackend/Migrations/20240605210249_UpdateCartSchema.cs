using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Carts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "CartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtTimeOfPurchase",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "4dca1209-5005-4d06-b956-6333bbc416c1", "AQAAAAIAAYagAAAAEJK/Am/EpLHIItFLQPduNWiHrgMPo+fB+Y+Pu3GpO71o4cRQzOJ603R9gwyfCuoD1w==", new DateTime(2024, 6, 5, 23, 2, 48, 623, DateTimeKind.Local).AddTicks(6908), "8fc68e9d-bd35-4b8a-b13b-fdad35ea70e4" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 5, 23, 2, 48, 623, DateTimeKind.Local).AddTicks(7212), new DateTime(2024, 5, 6, 23, 2, 48, 623, DateTimeKind.Local).AddTicks(7223) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 5, 23, 2, 48, 623, DateTimeKind.Local).AddTicks(7230), new DateTime(2024, 5, 26, 23, 2, 48, 623, DateTimeKind.Local).AddTicks(7233) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PriceAtTimeOfPurchase",
                table: "CartItems");

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
    }
}
