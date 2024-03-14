using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmForge.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AlterMovieTableAddingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Budget",
                table: "Movies",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Movies");
        }
    }
}
