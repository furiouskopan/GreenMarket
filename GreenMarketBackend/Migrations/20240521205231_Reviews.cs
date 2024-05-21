using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class Reviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Products_ProductId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProductId",
                table: "Reviews",
                newName: "IX_Reviews_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Review",
                newName: "IX_Review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProductId",
                table: "Review",
                newName: "IX_Review_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Review",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "ReviewId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234567-89ab-cdef-0123-456789abcdef",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "5ae8e346-1048-4b6c-9718-f618c607d3c4", "AQAAAAIAAYagAAAAEMQrTlHiGtPdpmNpCoy1G9wDKvTane6MBvmiAbt+BX+WRiu9nUh5aUGxk+aVFOoUCQ==", new DateTime(2024, 5, 17, 0, 28, 15, 165, DateTimeKind.Local).AddTicks(8790), "cbb58706-58ec-45b2-ae1b-86f034877e01" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 5, 17, 0, 28, 15, 165, DateTimeKind.Local).AddTicks(9063), new DateTime(2024, 4, 17, 0, 28, 15, 165, DateTimeKind.Local).AddTicks(9069) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "HarvestDate" },
                values: new object[] { new DateTime(2024, 5, 17, 0, 28, 15, 165, DateTimeKind.Local).AddTicks(9076), new DateTime(2024, 5, 7, 0, 28, 15, 165, DateTimeKind.Local).AddTicks(9078) });

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Products_ProductId",
                table: "Review",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
