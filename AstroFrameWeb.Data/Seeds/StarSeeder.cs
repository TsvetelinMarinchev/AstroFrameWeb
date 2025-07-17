using AstroFrameWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Seeds
{
    using System.Linq;
    public static class StarSeeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {

            var galaxy = dbContext.Galaxies.FirstOrDefault();
            var user = dbContext.Users.FirstOrDefault();
            var starType = dbContext.StarTypes.FirstOrDefault();


            if (galaxy == null || user == null || starType == null)
            {
                return;// nqmame potrebitel i galaktika
            }

            //iztritite povtarqshti se zvezdi
            //TODO 
            //var duplicateStars = dbContext.Stars
            //                 .AsEnumerable()
            //                 .GroupBy(s => s.Name)
            //                 .Where(g => g.Count() > 1)
            //                 .SelectMany(g => g.OrderBy(s => s.Id).Skip(1))
            //                 .ToList();

            //if (duplicateStars.Any())
            //{
            //    dbContext.Stars.RemoveRange(duplicateStars);
            //    dbContext.SaveChanges();
            //}
            // TODO:
            //var planets = dbContext.Planets.ToList();
            //dbContext.Planets.RemoveRange(planets);
            //dbContext.SaveChanges();
            //var allStars = dbContext.Stars.ToList();
            //dbContext.Stars.RemoveRange(allStars);
            //dbContext.SaveChanges();


            //TODO;Describe every Star with more information
            if (!dbContext.Stars.Any())
            {
               
                var stars = new[]
               {
                    new Star { Name = "Sun",
                        ImageUrl = "StarSun.jpg",
                        Description = "Our star",
                        Price = 0, CreatedOn = DateTime.UtcNow,
                        DiscoveredAgo= "4.6 billion years ago",
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id,
                        OwnerId = user.Id },
                    new Star { Name = "Alpha Centauri",
                        ImageUrl = "StarAlphaCentauri.jpg",
                        Description = "Closest star system",
                        Price = 10, CreatedOn = DateTime.UtcNow,
                        DiscoveredAgo= "4.1 billion years ago",
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id },
                    new Star { Name = "Betelgeuse",
                        ImageUrl = "StarBetelgeuse.jpg",
                        Description = "Red supergiant",
                        Price = 50, CreatedOn = DateTime.UtcNow,
                        DiscoveredAgo= "8.0 billion years ago",
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id },
                    new Star { Name = "Sirius",
                        ImageUrl = "StarSirius.jpg",
                        Description = "Brightest in the night sky",
                        Price = 30, CreatedOn = DateTime.UtcNow,
                        DiscoveredAgo= "0.2 billion years ago",
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id },
                    new Star { Name = "Vega",
                        ImageUrl = "StarVegaNew.jpg",
                        Description = "Very bright star",
                        Price = 25,
                        CreatedOn = DateTime.UtcNow,
                        DiscoveredAgo= "0.5 billion years ago",
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id }

                };
                dbContext.Stars.AddRange(stars);
                dbContext.SaveChanges();
            }
            
            
        }

    }
}

