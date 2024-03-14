using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmForge.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AlterActorTableAddingFeeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "Actors",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Actors");
        }
    }
}
