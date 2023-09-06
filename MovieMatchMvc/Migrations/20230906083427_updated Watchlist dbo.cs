using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieMatchMvc.Migrations
{
    /// <inheritdoc />
    public partial class updatedWatchlistdbo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Popularity",
                table: "watchLists",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "watchLists",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "watchLists");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "watchLists");
        }
    }
}
