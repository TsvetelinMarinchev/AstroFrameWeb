using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Enums;
using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace AstroFrameWeb.Tests.Services
{
    public class GalaxyServiceTest
    {

        [Fact]
        public async Task CreateGalaxyAsyncShouldAddGalaxyToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestGalaxyDb")
               .Options;
            var context = new ApplicationDbContext(options);
            var galaxyService = new GalaxyService(context);

            var model = new GalaxyCreateViewModel
            {
                Name = "Andromeda",
                Description = "Spiral galaxy",
                GalaxyType = GalaxyType.Spiral,
                NumberOfStars = 1000000,
                DistanceFromEarth = 2500000,
                ImageUrl = "https://example.com"
            };

            await galaxyService.CreateGalaxyAsync(model, "test-user");

            //prverka
            var galaxy = context.Galaxies.FirstOrDefault(g => g.Name == "Andromeda");

            Assert.NotNull(galaxy);
            Assert.Equal("test-user", galaxy.CreatorId);
            Assert.Equal(1000000, galaxy.NumberOfStars);
        }


    }
}
