using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroFrameWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscovoredAgoToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscoveredAgo",
                table: "Stars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscoveredAgo",
                table: "Planets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscoveredAgo",
                table: "Galaxies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscoveredAgo",
                table: "Stars");

            migrationBuilder.DropColumn(
                name: "DiscoveredAgo",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "DiscoveredAgo",
                table: "Galaxies");
        }
    }
}
