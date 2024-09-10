using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class ProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "faa7b9a4-8ff5-49ac-8f0d-8e456dd99293", "AQAAAAIAAYagAAAAEJSgUAhfGyr5hSPO8A0EoKvPOzRLW1nOseD8oMXlwCGNd9ZldxdBP4zPWfIxig5qxA==", new DateTime(2024, 9, 10, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7500), "732dc6ba-75a7-4695-8672-f061b1dfe6e8" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 10, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7805), new DateTime(2024, 8, 11, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7810) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 9, 10, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7817), new DateTime(2024, 8, 31, 17, 49, 27, 651, DateTimeKind.Local).AddTicks(7819) });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImage");

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
    }
}
