using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstroFrameWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrateNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StarTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "PK for StarType")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "The type of StarType"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Optional describe for StarType")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Galaxies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for Galaxy")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The name of the Galaxy"),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Description of the Galaxy"),
                    GalaxyType = table.Column<int>(type: "int", nullable: false, comment: "The type of Galaxy"),
                    NumberOfStars = table.Column<int>(type: "int", nullable: false, comment: "Total number of stars in the Galaxy"),
                    DistanceFromEarth = table.Column<double>(type: "float", nullable: false, comment: "Distance of the Galaxy from Earth in light years"),
                    DiscoveredOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when the Galaxy was discovered or added"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "FK to the user who created this Galaxy")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galaxies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Galaxies_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Shows name of the star"),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Information for Star"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Shows the price of the Star"),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false, comment: "Shows the star if it has been purchased"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Shows real time now"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "FK to the user who owns the star"),
                    GalaxyId = table.Column<int>(type: "int", nullable: false, comment: "Galaxy the star belongs to"),
                    StarTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stars_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stars_Galaxies_GalaxyId",
                        column: x => x.GalaxyId,
                        principalTable: "Galaxies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stars_StarTypes_StarTypeId",
                        column: x => x.StarTypeId,
                        principalTable: "StarTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for Planet")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The name of the planet"),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Detailed description of the planet"),
                    Mass = table.Column<double>(type: "float", nullable: false, comment: "Mass of the planet in Earth masses"),
                    Radius = table.Column<double>(type: "float", nullable: false, comment: "Radius of the planet in Earth radiius"),
                    DistanceFromEarth = table.Column<double>(type: "float", nullable: false, comment: "Distance of the planet from Earth in light years"),
                    DiscoveredOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Discovery date"),
                    StarId = table.Column<int>(type: "int", nullable: false, comment: "FK to the Star"),
                    GalaxyId = table.Column<int>(type: "int", nullable: true, comment: "FK to the Galaxy"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "FK to the creator")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planets_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Planets_Galaxies_GalaxyId",
                        column: x => x.GalaxyId,
                        principalTable: "Galaxies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Planets_Stars_StarId",
                        column: x => x.StarId,
                        principalTable: "Stars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StarComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "User comment for a star"),
                    StarId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StarComments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StarComments_Stars_StarId",
                        column: x => x.StarId,
                        principalTable: "Stars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanetComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Comment identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "The content of the comment"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanetComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanetComments_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoritePlanets",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoritePlanets", x => new { x.UserId, x.PlanetId });
                    table.ForeignKey(
                        name: "FK_UserFavoritePlanets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFavoritePlanets_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Galaxies_CreatorId",
                table: "Galaxies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetComments_PlanetId",
                table: "PlanetComments",
                column: "PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetComments_UserId",
                table: "PlanetComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_CreatorId",
                table: "Planets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_GalaxyId",
                table: "Planets",
                column: "GalaxyId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_StarId",
                table: "Planets",
                column: "StarId");

            migrationBuilder.CreateIndex(
                name: "IX_StarComments_AuthorId",
                table: "StarComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_StarComments_StarId",
                table: "StarComments",
                column: "StarId");

            migrationBuilder.CreateIndex(
                name: "IX_Stars_GalaxyId",
                table: "Stars",
                column: "GalaxyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stars_OwnerId",
                table: "Stars",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stars_StarTypeId",
                table: "Stars",
                column: "StarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoritePlanets_PlanetId",
                table: "UserFavoritePlanets",
                column: "PlanetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PlanetComments");

            migrationBuilder.DropTable(
                name: "StarComments");

            migrationBuilder.DropTable(
                name: "UserFavoritePlanets");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Stars");

            migrationBuilder.DropTable(
                name: "Galaxies");

            migrationBuilder.DropTable(
                name: "StarTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
