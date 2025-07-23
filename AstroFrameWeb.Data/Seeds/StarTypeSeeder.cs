using AstroFrameWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Data.Seeds
{
    public static class StarTypeSeeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {

            var planets =
                dbContext.Planets.ToList();
                dbContext.Planets.RemoveRange(planets);
                dbContext.SaveChanges();
            var stars =
                dbContext.Stars.ToList();
                dbContext.Stars.RemoveRange(stars);
                dbContext.SaveChanges();
            var starType = 
                dbContext.StarTypes.ToList();
                dbContext.StarTypes.RemoveRange(starType);
               dbContext.SaveChanges();


            if (!dbContext.StarTypes.Any())
            {
                var types = new[]
                {
                    new StarType { Name = "G-type", ImageUrl = "StarTypeG.jpg", Description = "Like the Sun" },
                    new StarType { Name = "M-type", ImageUrl = "StarTypeM.jpg", Description = "Cool red dwarfs" },
                    new StarType { Name = "O-type", ImageUrl = "StarTypeO.jpg", Description = "Hot and massive" },
                    new StarType { Name = "B-type", ImageUrl = "StarTypeB.jpg", Description = "Blue and bright" },
                    new StarType { Name = "K-type", ImageUrl = "StarTypeK.jpg", Description = "Orange main sequence" }
                };

                dbContext.StarTypes.AddRange(types);
                dbContext.SaveChanges();
            }
            
           
        }
    }
}
