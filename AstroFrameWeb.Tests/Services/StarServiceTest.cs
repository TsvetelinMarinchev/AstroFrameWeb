using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.Data;
using AstroFrameWeb.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstroFrameWeb.Data.Models;
using AstroFrameWeb.Data.Enums;

namespace AstroFrameWeb.Tests.Services
{
    public class StarServiceTest
    {
        [Fact]
        public async Task CreateStarAsyncShouldAddStarToDatabaseWhenModelIsValid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Create_Valid_Star")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new StarService(context);

            var model = new StarCreateViewModel
            {
                Name = "Sirius",
                Description = "Brightest star",
                Price = 999.99m,
                GalaxyId = 1,
                StarTypeId = 1,
                ImageUrl = "https://image.com/star.png"
            };

            await service.CreateStarAsync(model, "user-1");

            var count = context.Stars.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CreateStarAsyncShouldNotAddStarToDatabaseWhenModelIsInvalid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Create_Invalid_Star")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new StarService(context);

            var model = new StarCreateViewModel
            {
                Name = "", // bez modeli
                Description = "", 
                Price = -5, 
                GalaxyId = 0, 
                StarTypeId = 0,
                ImageUrl = "invalid-url"
            };

            await service.CreateStarAsync(model, "user-1");

            var count = context.Stars.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllStars()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetAllStarsDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var galaxy = new Galaxy { Name = "Milky Way", Description = "Test galaxy", GalaxyType = GalaxyType.Spiral };
            var starType = new StarType { Name = "G-type", Description = "Yellow dwarf" };

            context.Galaxies.Add(galaxy);
            context.StarTypes.Add(starType);
            await context.SaveChangesAsync();

            context.Stars.AddRange(
                new Star 
                {
                    Name = "Alpha",
                    Description = "Star A",
                    Price = 1,
                    GalaxyId = galaxy.Id,
                    StarTypeId = starType.Id,
                    ImageUrl = "https://a.com"

                },
                new Star
                {
                    Name = "Beta",
                    Description = "Star B",
                    Price = 2,
                    GalaxyId = galaxy.Id,
                    StarTypeId = starType.Id,
                    ImageUrl = "https://b.com"
                }
            );
            await context.SaveChangesAsync();

            var service = new StarService(context);
            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, s => s.Name == "Alpha");
            Assert.Contains(result, s => s.Name == "Beta");
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectStar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetByIdStarDb")
                .Options;

            using var context = new ApplicationDbContext(options);
       
            var galaxy = new Galaxy { Name = "Milky Way", Description = "Test galaxy", GalaxyType = GalaxyType.Spiral };
            var starType = new StarType { Name = "G-type", Description = "Yellow dwarf" };

            context.Galaxies.Add(galaxy);
            context.StarTypes.Add(starType);
            await context.SaveChangesAsync();

            var star = new Star
            {
                Name = "Vega",
                Description = "Bright star",
                Price = 300,
                GalaxyId = 1,
                StarTypeId = 1,
                ImageUrl = "https://vega.com"
            };

            context.Stars.Add(star);
            await context.SaveChangesAsync();

            var service = new StarService(context);
            var result = await service.GetByIdAsync(star.Id);

            Assert.NotNull(result);
            Assert.Equal("Vega", result.Name);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullWhenStarNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetByIdNullStarDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new StarService(context);

            var result = await service.GetByIdAsync(999);
            Assert.Null(result);
        }


        [Fact]
        public async Task UpdateStarAsyncShouldUpdateStarFields()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UpdateStarDb")
                .Options;

            using var context = new ApplicationDbContext(options);

            var star = new Star
            {
                Name = "Old",
                Description = "Old Desc",
                Price = 10,
                GalaxyId = 1,
                StarTypeId = 1,
                ImageUrl = "https://old.com"
            };

            context.Stars.Add(star);
            await context.SaveChangesAsync();

            var service = new StarService(context);

            var updatedModel = new StarCreateViewModel
            {
                Name = "New",
                Description = "New Desc",
                Price = 999,
                GalaxyId = 2,
                StarTypeId = 2,
                ImageUrl = "https://new.com"
            };

            await service.UpdateStarAsync(star.Id, updatedModel);

            var updated = await context.Stars.FindAsync(star.Id);
            Assert.Equal("New", updated.Name);
            Assert.Equal("New Desc", updated.Description);
            Assert.Equal(999, updated.Price);
            Assert.Equal("https://new.com", updated.ImageUrl);
        }

        [Fact]
        public async Task DeleteStarAsyncShouldRemoveStar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DeleteStarDb")
                .Options;

            using var context = new ApplicationDbContext(options);

            var star = new Star
            {
                Name = "ToDelete",
                Description = "This one goes",
                Price = 100,
                GalaxyId = 1,
                StarTypeId = 1,
                ImageUrl = "https://delete.com"
            };

            context.Stars.Add(star);
            await context.SaveChangesAsync();

            var service = new StarService(context);
            await service.DeleteStarAsync(star.Id);

            var deleted = await context.Stars.FindAsync(star.Id);
            Assert.Null(deleted);
        }
    }
}
