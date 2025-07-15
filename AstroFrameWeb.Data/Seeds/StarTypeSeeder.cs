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
            if (!dbContext.StarTypes.Any())
            {
                var types = new[]
                {
                    new StarType { Name = "G-type", ImageUrl = "/images/StartypeG", Description = "Like the Sun" },
                    new StarType { Name = "M-type", ImageUrl = "/images/StartypeM", Description = "Cool red dwarfs" },
                    new StarType { Name = "O-type", ImageUrl = "/images/StartypeO", Description = "Hot and massive" },
                    new StarType { Name = "B-type", ImageUrl = "/images/StartypeB", Description = "Blue and bright" },
                    new StarType { Name = "K-type", ImageUrl = "/images/StartypeK", Description = "Orange main sequence" }
                };

                dbContext.StarTypes.AddRange(types);
            }
            
            dbContext.SaveChanges();
        }
    }
}
