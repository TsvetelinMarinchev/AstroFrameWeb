using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroFrameWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlForEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "StarTypes",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Local image file for the TypeOfStar");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Stars",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Local image file for the star");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Planets",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Local image file for the Planet");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Galaxies",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Local image file for the Galaxy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "StarTypes");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Stars");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Galaxies");
        }
    }
}
