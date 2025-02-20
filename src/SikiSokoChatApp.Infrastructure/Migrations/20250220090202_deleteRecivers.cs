using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SikiSokoChatApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteRecivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_users_ReciverId",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "IX_messages_ReciverId",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "ReciverId",
                table: "messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReciverId",
                table: "messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_messages_ReciverId",
                table: "messages",
                column: "ReciverId");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_users_ReciverId",
                table: "messages",
                column: "ReciverId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
