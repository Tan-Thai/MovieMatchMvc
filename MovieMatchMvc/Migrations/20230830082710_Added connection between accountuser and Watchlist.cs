using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatchMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddedconnectionbetweenaccountuserandWatchlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountUserId",
                table: "watchLists",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_watchLists_AccountUserId",
                table: "watchLists",
                column: "AccountUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_watchLists_AspNetUsers_AccountUserId",
                table: "watchLists",
                column: "AccountUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_watchLists_AspNetUsers_AccountUserId",
                table: "watchLists");

            migrationBuilder.DropIndex(
                name: "IX_watchLists_AccountUserId",
                table: "watchLists");

            migrationBuilder.DropColumn(
                name: "AccountUserId",
                table: "watchLists");
        }
    }
}
