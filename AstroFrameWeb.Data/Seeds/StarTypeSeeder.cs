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
                    new StarType { Name = "G-type", Description = "Like the Sun" },
                    new StarType { Name = "M-type", Description = "Cool red dwarfs" },
                    new StarType { Name = "O-type", Description = "Hot and massive" },
                    new StarType { Name = "B-type", Description = "Blue and bright" },
                    new StarType { Name = "K-type", Description = "Orange main sequence" }
                };

                dbContext.StarTypes.AddRange(types);
            }
            
            dbContext.SaveChanges();
        }
    }
}
