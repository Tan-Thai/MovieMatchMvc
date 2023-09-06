using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatchMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddedMovieGenrestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Popularity",
                table: "watchLists",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "movieGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    WatchListId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movieGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_movieGenres_watchLists_WatchListId",
                        column: x => x.WatchListId,
                        principalTable: "watchLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_movieGenres_WatchListId",
                table: "movieGenres",
                column: "WatchListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movieGenres");

            migrationBuilder.AlterColumn<double>(
                name: "Popularity",
                table: "watchLists",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
