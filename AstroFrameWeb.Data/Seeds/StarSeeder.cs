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

            if(!dbContext.Stars.Any(s => s.Name == "Sun"))
            {
                var galaxy = dbContext.Galaxies.FirstOrDefault();
                var user = dbContext.Users.FirstOrDefault();

                if(galaxy == null || user == null)
                {
                    return;// nqmame potrebitel i galaktika
                }

                var sun = new Star
                {
                    Name = "Sun",
                    Description= "The Sun is the star at the centre of the Solar System.",
                    Price=0,
                    IsPurchased = false,
                    CreatedOn = DateTime.UtcNow,
                    OwnerId = user.Id,
                    GalaxyId = galaxy.Id
                };
                dbContext.Stars.Add(sun);
                dbContext.SaveChanges();
            }

        }

    }
}
