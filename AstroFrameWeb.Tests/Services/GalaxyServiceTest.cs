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

        [Fact]
        public async Task CreateGalaxyToTheDatabaseWhenNameIsMissing()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: "InvalidGalaxyDb")
                        .Options;


            var context = new ApplicationDbContext(options);
            var galaxyService = new GalaxyService(context);

            var model = new GalaxyCreateViewModel
            {
                //bez Name // testing
                Description = "Galaxy with no name",
                GalaxyType = GalaxyType.Irregular,
                NumberOfStars = 12345,
                DistanceFromEarth = 1500000,
                ImageUrl = "https://example.com/image.png"
            };
            await galaxyService.CreateGalaxyAsync(model, "test-user");

            var count = context.Galaxies.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CreateGalaxySholdNotSavedWhenTheNumberOfStarsIsZero()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                           .UseInMemoryDatabase("StarsInvalid")
                           .Options;
            var context = new ApplicationDbContext(options);
            var galaxyService = new GalaxyService(context);

            var model = new GalaxyCreateViewModel
            {
                Name = "Ghost Galaxy",
                Description = "Invisible galaxy",
                GalaxyType = GalaxyType.Irregular,
                NumberOfStars = 0,
                DistanceFromEarth = 500000,
                ImageUrl = "https://ghost.com/img.png"
            };
            await galaxyService.CreateGalaxyAsync(model, "test-user");

            var count = context.Galaxies.Count();
            Assert.Equal(0, count);
        }


        [Fact]
        public async Task CreateGalaxyShouldNotSaveWhenDistanceIsNegativeOrZero()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                       .UseInMemoryDatabase(databaseName: "Galaxy_InvalidDistance")
                       .Options;

            var context = new ApplicationDbContext(options);
            var galaxyService = new GalaxyService(context);

            var model = new GalaxyCreateViewModel
            {
                Name = "Dark Void",
                Description = "A galaxy with suspicious distance",
                GalaxyType = GalaxyType.Irregular,
                NumberOfStars = 9999,
                DistanceFromEarth = -1000,
                ImageUrl = "https://darkvoid.com/image.png"
            };

            await galaxyService.CreateGalaxyAsync(model, "test-user");

            var count = context.Galaxies.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CreateGalaxyShouldNotSaveWhenImageUrlIsInvalid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: "Galaxy_InvalidUrl")
                      .Options;

            var context = new ApplicationDbContext(options);
            var galaxyService = new GalaxyService(context);

            var model = new GalaxyCreateViewModel
            {
                Name = "Glitchy Galaxy",
                Description = "A mysterious object",
                GalaxyType = GalaxyType.Irregular,
                NumberOfStars = 1000,
                DistanceFromEarth = 12345,
                ImageUrl = "not-a-url"
            };
            await galaxyService.CreateGalaxyAsync(model, "test-user");

            var count = context.Galaxies.Count();
            Assert.Equal(0, count);
        }

    }
}
