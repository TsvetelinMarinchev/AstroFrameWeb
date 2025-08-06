using System;
//using System.Linq;
//using System.Threading.Tasks;
using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Enums;
using AstroFrameWeb.Data.Models;
using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.Services.Implementations;
using Microsoft.EntityFrameworkCore;

using AstroFrameWeb.Services.Mapping;
using AutoMapper;

namespace AstroFrameWeb.Tests.Services
{
    public class GalaxyServiceTest
    {
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
               cfg.AddProfile<AutoMapperProfile>();
            });
           return new Mapper(config);
        }

        [Fact]
        public async Task CreateGalaxyAsyncShouldAddGalaxyToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestGalaxyDb")
               .Options;
            var context = new ApplicationDbContext(options);
            var mapper = GetMapper();
            var galaxyService = new GalaxyService(context, mapper);

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
            var mapper = GetMapper();
            var galaxyService = new GalaxyService(context, mapper);

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
            var mapper = GetMapper();
            var galaxyService = new GalaxyService(context, mapper);

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
            var mapper = GetMapper();
            var galaxyService = new GalaxyService(context, mapper);

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
            var mapper = GetMapper();
            var galaxyService = new GalaxyService(context, mapper);

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

        [Fact]
        public async Task GetAllAsyncShouldReturnAllGalaxies()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetAllGalaxiesDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mapper = GetMapper();

            context.Galaxies.AddRange(
                new Galaxy { Name = "Milky Way", Description = "Home", GalaxyType = GalaxyType.Spiral, NumberOfStars = 100000, DistanceFromEarth = 0 },
                new Galaxy { Name = "Andromeda", Description = "Neighbor", GalaxyType = GalaxyType.Spiral, NumberOfStars = 200000, DistanceFromEarth = 2500000 }
            );
            await context.SaveChangesAsync();

            var service = new GalaxyService(context, mapper);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, g => g.Name == "Milky Way");
            Assert.Contains(result, g => g.Name == "Andromeda");
        }


        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectGalaxy()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetByIdGalaxyDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mapper = GetMapper();

            var galaxy = new Galaxy
            {
                Name = "Whirlpool",
                Description = "Spiral beauty",
                GalaxyType = GalaxyType.Spiral,
                NumberOfStars = 50000,
                DistanceFromEarth = 23000000
            };

            context.Galaxies.Add(galaxy);
            await context.SaveChangesAsync();

            var service = new GalaxyService(context, mapper);
            var result = await service.GetByIdAsync(galaxy.Id);

            Assert.NotNull(result);
            Assert.Equal("Whirlpool", result.Name);
        }


        [Fact]
        public async Task GetByIdAsyncShouldReturnNullWhenGalaxyDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetByIdNullGalaxyDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mapper = GetMapper();

            var service = new GalaxyService(context, mapper);
            var result = await service.GetByIdAsync(999);
            Assert.Null(result);

        }

        //update galaxy
        [Fact]
        public async Task UpdateGalaxyAsyncShouldUpdateGalaxyFields()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UpdateGalaxyDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mapper = GetMapper();

            var galaxy = new Galaxy
            {
                Name = "Old Name",
                Description = "Old Description",
                GalaxyType = GalaxyType.Spiral,
                NumberOfStars = 100000,
                DistanceFromEarth = 1000000,
                ImageUrl = "https://old.com"
            };

            context.Galaxies.Add(galaxy);
            await context.SaveChangesAsync();

            var service = new GalaxyService(context, mapper);
            var updatedModel = new GalaxyCreateViewModel
            {
                Name = "New Name",
                Description = "New Description",
                GalaxyType = GalaxyType.Elliptical,
                NumberOfStars = 999999,
                DistanceFromEarth = 2000000,
                ImageUrl = "https://new.com"
            };

            await service.UpdateGalaxyAsync(galaxy.Id, updatedModel);

            var updated = await context.Galaxies.FindAsync(galaxy.Id);

            Assert.Equal("New Name", updated.Name);
            Assert.Equal("New Description", updated.Description);
            Assert.Equal(GalaxyType.Elliptical, updated.GalaxyType);
            Assert.Equal(999999, updated.NumberOfStars);
            Assert.Equal("https://new.com", updated.ImageUrl);
        }

        [Fact]
        public async Task UpdateGalaxyAsyncShouldNotThrowWhenGalaxyNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UpdateNonExistingGalaxyDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mapper = GetMapper();
            var service = new GalaxyService(context, mapper);

            var updatedModel = new GalaxyCreateViewModel
            {
                Name = "Doesn't Matter",
                Description = "Nope",
                GalaxyType = GalaxyType.Elliptical,
                NumberOfStars = 1,
                DistanceFromEarth = 1,
                ImageUrl = "https://none.com"
            };

            await service.UpdateGalaxyAsync(999, updatedModel);
            var galaxies = await context.Galaxies.ToListAsync();
            Assert.Empty(galaxies);
        }

        [Fact]
        public async Task DeleteGalaxyAsyncShouldRemoveGalaxy()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DeleteGalaxyDb")
                .Options;

            using var context = new ApplicationDbContext(options);

            var mapper = GetMapper();

            var galaxy = new Galaxy
            {
                Name = "Temp Galaxy",
                Description = "Temporary galaxy for testing",
                GalaxyType = GalaxyType.Spiral,
                NumberOfStars = 1,
                DistanceFromEarth = 1000,
                ImageUrl = "https://temp.com/img.png"
            };
            context.Galaxies.Add(galaxy);
            await context.SaveChangesAsync();

            var service = new GalaxyService(context, mapper);
            await service.DeleteGalaxyAsync(galaxy.Id);

            var deleted = await context.Galaxies.FindAsync(galaxy.Id);
            Assert.Null(deleted);
        }
    }
}
