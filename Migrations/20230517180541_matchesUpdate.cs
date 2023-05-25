using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchNow.Migrations
{
    /// <inheritdoc />
    public partial class matchesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "toId",
                table: "Matches",
                newName: "ToId");

            migrationBuilder.RenameColumn(
                name: "fromId",
                table: "Matches",
                newName: "FromId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToId",
                table: "Matches",
                newName: "toId");

            migrationBuilder.RenameColumn(
                name: "FromId",
                table: "Matches",
                newName: "fromId");
        }
    }
}
