using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatchMvc.Migrations
{
    /// <inheritdoc />
    public partial class addeduserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_watchLists_AspNetUsers_AccountUserId",
                table: "watchLists");

            migrationBuilder.AlterColumn<string>(
                name: "AccountUserId",
                table: "watchLists",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_watchLists_AspNetUsers_AccountUserId",
                table: "watchLists",
                column: "AccountUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_watchLists_AspNetUsers_AccountUserId",
                table: "watchLists");

            migrationBuilder.AlterColumn<string>(
                name: "AccountUserId",
                table: "watchLists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_watchLists_AspNetUsers_AccountUserId",
                table: "watchLists",
                column: "AccountUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
