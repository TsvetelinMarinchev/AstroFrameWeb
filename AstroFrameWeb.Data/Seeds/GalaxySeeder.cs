using AstroFrameWeb.Data.Enums;
using AstroFrameWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Seeds
{
    public static class GalaxySeeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            if (!dbContext.Galaxies.Any())
            {
                var galaxies = new[]
                {
                    new Galaxy { Name = "Milky Way",
                        ImageUrl = "/images/GalaxyMilkyWay",
                        Description = "Our galaxy",
                        GalaxyType = GalaxyType.Spiral,
                        NumberOfStars = 10000000,
                        DistanceFromEarth = 0,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "13.5 billion years ago"},
  
                    new Galaxy { Name = "Andromeda",
                        ImageUrl = "/images/GalaxyAndromeda",
                        Description = "Closest spiral neighbor",
                        GalaxyType = GalaxyType.Spiral,
                        NumberOfStars = 100000000,
                        DistanceFromEarth = 2537000,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "10.2 billion years ago"},

                    new Galaxy { Name = "Sombrero",
                        ImageUrl = "/images/GalaxySombrero",
                        Description = "Hat-shaped galaxy",
                        GalaxyType = GalaxyType.Spiral,
                        NumberOfStars = 800000000,
                        DistanceFromEarth = 29000000,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "8.2 billion years ago"},

                    new Galaxy { Name = "Messier 87",
                        ImageUrl = "/images/GalaxyMessier87",
                        Description = "Contains supermassive black hole",
                        GalaxyType = GalaxyType.Elliptical,
                        NumberOfStars = 1200000000,
                        DistanceFromEarth = 53000000,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "11.4 billion years ago"},

                    new Galaxy { Name = "Triangulum",
                        ImageUrl = "/images/GalaxyTriangulum",
                        Description = "Part of local group",
                        GalaxyType = GalaxyType.Spiral,
                        NumberOfStars = 400000000,
                        DistanceFromEarth = 2730000,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "11.9 billion years ago"}
                };

                dbContext.Galaxies.AddRange(galaxies);
                dbContext.SaveChanges();

            }
        }
    }
}
