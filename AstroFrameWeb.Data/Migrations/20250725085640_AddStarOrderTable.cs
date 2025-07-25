using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroFrameWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStarOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StarOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StarId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BonusUnlocked = table.Column<bool>(type: "bit", nullable: false, comment: "Shows the bonus is locked or not")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarOrders_Stars_StarId",
                        column: x => x.StarId,
                        principalTable: "Stars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StarOrders_StarId",
                table: "StarOrders",
                column: "StarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StarOrders");
        }
    }
}
