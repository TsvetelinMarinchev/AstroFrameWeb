using AstroFrameWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Seeds
{
    public static class PlanetSeeder
    {

        public static void Seed(ApplicationDbContext dbContext)
        {
            if (!dbContext.Planets.Any())
            {
                var star = dbContext.Stars.FirstOrDefault();
                var galaxy = dbContext.Galaxies.FirstOrDefault();
                var user = dbContext.Users.FirstOrDefault();

                if (star == null || galaxy == null || user == null)
                    return;

                var planets = new[]
                {
                    new Planet { Name = "Mercury",
                        ImageUrl = "/images/PlanetMercury",
                        Description = "Closest planet to Sun",
                        Mass = 0.055, Radius = 0.383,
                        DistanceFromEarth = 77,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "5.1 billion years ago",
                        StarId = star.Id, GalaxyId = galaxy.Id,
                        CreatorId = user.Id },
                    new Planet { Name = "Venus",
                        ImageUrl = "/images/PlanetVenus",
                        Description = "Similar to Earth",
                        Mass = 0.815, Radius = 0.949,
                        DistanceFromEarth = 261,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "4.1 billion years ago",
                        StarId = star.Id, GalaxyId = galaxy.Id,
                        CreatorId = user.Id },
                    new Planet { Name = "Earth",
                        ImageUrl = "/images/PlanetEarth",
                        Description = "Our home",
                        Mass = 1.0, Radius = 1.0,
                        DistanceFromEarth = 0,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "4.6 billion years ago",
                        StarId = star.Id, GalaxyId = galaxy.Id,
                        CreatorId = user.Id },
                    new Planet { Name = "Mars",
                        ImageUrl = "/images/PlanetMars",
                        Description = "The red planet",
                        Mass = 0.107, Radius = 0.532,
                        DistanceFromEarth = 225,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "3.9 billion years ago",
                        StarId = star.Id, GalaxyId = galaxy.Id,
                        CreatorId = user.Id },
                    new Planet { Name = "Jupiter",
                        ImageUrl = "/images/PlanetJupiter",
                        Description = "Largest planet",
                        Mass = 317.8, Radius = 11.21,
                        DistanceFromEarth = 778,
                        DiscoveredOn = DateTime.UtcNow,
                        DiscoveredAgo = "4.6 billion years ago",
                        StarId = star.Id,
                        GalaxyId = galaxy.Id,
                        CreatorId = user.Id }
                };

                dbContext.Planets.AddRange(planets);
                dbContext.SaveChanges();
            }
        }
    }
}
