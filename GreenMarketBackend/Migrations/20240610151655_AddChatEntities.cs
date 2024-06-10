using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenMarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddChatEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatSessions",
                columns: table => new
                {
                    ChatSessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.ChatSessionId);
                    table.ForeignKey(
                        name: "FK_ChatSessions_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatSessions_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatSessionId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_ChatSessions_ChatSessionId",
                        column: x => x.ChatSessionId,
                        principalTable: "ChatSessions",
                        principalColumn: "ChatSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_User1Id",
                table: "ChatSessions",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_User2Id",
                table: "ChatSessions",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatSessionId",
                table: "Messages",
                column: "ChatSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ChatSessions");

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
    }
}
