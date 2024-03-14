using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmForge.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AlterInvitationTableAddInviteTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitationType",
                table: "Invitations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationType",
                table: "Invitations");
        }
    }
}
