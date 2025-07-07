using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;

namespace AstroFrameWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFavoritePlanet>()
                .HasKey(x => new { x.UserId, x.PlanetId });

            builder.Entity<UserFavoritePlanet>()
                .HasOne(x => x.User)
                .WithMany(u => u.FavoritePlanets)
                .HasForeignKey(x => x.UserId);

            builder.Entity<UserFavoritePlanet>()
                .HasOne(x => x.Planet)
                .WithMany(p => p.FavoritedByUsers)
                .HasForeignKey(x => x.PlanetId);
        }
        public DbSet<Galaxy> Galaxies { get; set; } = null!;
        public DbSet<Star> Stars { get; set; } = null!;
        public DbSet<Planet> Planets { get; set; } = null!;
        public DbSet<StarComment> StarComments { get; set; } = null!;
        public DbSet<PlanetComment> PlanetComments { get; set; } = null!;
        public DbSet<UserFavoritePlanet> UserFavoritePlanets { get; set; } = null!;
    }
}
