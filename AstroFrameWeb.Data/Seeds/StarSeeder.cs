using AstroFrameWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Seeds
{
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
            var stars = new[]
           {
                    new Star { Name = "Sun",
                        Description = "Our star",
                        Price = 0, CreatedOn = DateTime.UtcNow,
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id,
                        OwnerId = user.Id },
                    new Star { Name = "Alpha Centauri",
                        Description = "Closest star system",
                        Price = 10, CreatedOn = DateTime.UtcNow,
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id },
                    new Star { Name = "Betelgeuse",
                        Description = "Red supergiant",
                        Price = 50, CreatedOn = DateTime.UtcNow,
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id },
                    new Star { Name = "Sirius",
                        Description = "Brightest in the night sky",
                        Price = 30, CreatedOn = DateTime.UtcNow,
                        GalaxyId = galaxy.Id,
                        StarTypeId = starType.Id },
                    new Star { Name = "Vega", Description = "Very bright star", Price = 25, CreatedOn = DateTime.UtcNow, GalaxyId = galaxy.Id, StarTypeId = starType.Id }
                };

            var sun = new Star
            {
                Name = "Sun",
                Description = "The Sun is the star at the centre of the Solar System.",
                Price = 0,
                IsPurchased = false,
                CreatedOn = DateTime.UtcNow,
                OwnerId = user.Id,
                GalaxyId = galaxy.Id
            };
            dbContext.Stars.AddRange(stars);
            dbContext.SaveChanges();


        }

    }
}

