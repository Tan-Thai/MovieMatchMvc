using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatchMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddedMovieId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "watchLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "watchLists");
        }
    }
}
