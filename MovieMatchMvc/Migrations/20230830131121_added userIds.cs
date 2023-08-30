using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatchMvc.Migrations
{
    /// <inheritdoc />
    public partial class addeduserIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "watchLists",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "watchLists");
        }
    }
}
