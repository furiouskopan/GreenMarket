using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class Payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "PriceAtTimeOfPurchase");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "9127f4c1-a2ec-43b3-8036-a6dc1a4612dc", "AQAAAAIAAYagAAAAEKzWe+e4Fyp+3ewe5KjOZuLaD+T1PcItbleUanJzz332gZSH0sXn3SGLMixJjz41wg==", new DateTime(2024, 6, 9, 19, 1, 25, 814, DateTimeKind.Local).AddTicks(446), "5b5fd63d-c218-4268-ac2a-a7763cfe1b68" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 9, 19, 1, 25, 814, DateTimeKind.Local).AddTicks(750), new DateTime(2024, 5, 10, 19, 1, 25, 814, DateTimeKind.Local).AddTicks(754) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 6, 9, 19, 1, 25, 814, DateTimeKind.Local).AddTicks(762), new DateTime(2024, 5, 30, 19, 1, 25, 814, DateTimeKind.Local).AddTicks(764) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PriceAtTimeOfPurchase",
                table: "OrderItems",
                newName: "UnitPrice");

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
    }
}
