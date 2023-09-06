using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatchMvc.Migrations
{
    /// <inheritdoc />
    public partial class cascadingdeletionwatchlistintogenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movieGenres_watchLists_WatchListId",
                table: "movieGenres");

            migrationBuilder.AddForeignKey(
                name: "FK_movieGenres_watchLists_WatchListId",
                table: "movieGenres",
                column: "WatchListId",
                principalTable: "watchLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movieGenres_watchLists_WatchListId",
                table: "movieGenres");

            migrationBuilder.AddForeignKey(
                name: "FK_movieGenres_watchLists_WatchListId",
                table: "movieGenres",
                column: "WatchListId",
                principalTable: "watchLists",
                principalColumn: "Id");
        }
    }
}
