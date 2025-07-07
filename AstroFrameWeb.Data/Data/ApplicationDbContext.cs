using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;


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
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserFavoritePlanet>()
                .HasOne(x => x.Planet)
                .WithMany(p => p.FavoritedByUsers)
                .HasForeignKey(x => x.PlanetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Galaxy>()
                .HasMany(g => g.Stars)
                .WithOne(s => s.Galaxy)
                .HasForeignKey(s => s.GalaxyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Star>()
                .HasMany(s => s.Planets)
                .WithOne(p => p.Star)
                .HasForeignKey( p => p.StarId)
                .OnDelete(DeleteBehavior.Restrict);
           
            builder.Entity<Planet>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Planet)
                 .HasForeignKey(c => c.PlanetId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Star>()
                  .HasMany(s => s.Comments)
                  .WithOne(c => c.Star)
                  .HasForeignKey(c => c.StarId)
                  .OnDelete(DeleteBehavior.Restrict);
           
        }
        public DbSet<Galaxy> Galaxies { get; set; } = null!;
        public DbSet<Star> Stars { get; set; } = null!;
        public DbSet<Planet> Planets { get; set; } = null!;
        public DbSet<StarComment> StarComments { get; set; } = null!;
        public DbSet<PlanetComment> PlanetComments { get; set; } = null!;
        public DbSet<UserFavoritePlanet> UserFavoritePlanets { get; set; } = null!;
    }
}
