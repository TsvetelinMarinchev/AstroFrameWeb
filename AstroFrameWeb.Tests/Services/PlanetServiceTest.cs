using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.Services.Implementations;
using AstroFrameWeb.Services.Mapping;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Tests.Services
{
    public class PlanetServiceTest
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
        public async Task CreatePlanetAsyncShouldAddPlanetToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CreatePlanetTestDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var galaxy = new Galaxy { Name = "Test Galaxy", Description = "Test galaxy" };
            var star = new Star { Name = "Test Star", Description = "Some description", Galaxy = galaxy };
            context.Galaxies.Add(galaxy);
            context.Stars.Add(star);
            await context.SaveChangesAsync();
            var mapper = GetMapper();
            var service = new PlanetService(context, mapper);

            var model = new PlanetCreateViewModel
            {
                Name = "Mars",
                Description = "Red Planet",
                Mass = 0.11,
                Radius = 0.53,
                DistanceFromEarth = 54.6,
                ImageUrl = "https://mars.com/img.png",
                GalaxyId = galaxy.Id,
                StarId = star.Id
            };

            await service.CreatePlanetAsync(model, "test-user");
            var planet = await context.Planets.FirstOrDefaultAsync(p => p.Name == "Mars");
            Assert.NotNull(planet);
            Assert.Equal("test-user", planet.CreatorId);
        }


        [Fact]
        public async Task CreatePlanetShouldNotAddToDatabaseWhenModelIsInvalid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                     .UseInMemoryDatabase("InvalidPlanetDb")
                     .Options;

            using var context = new ApplicationDbContext(options);
            var mapper = GetMapper();
            var service = new PlanetService(context, mapper);

            var invalidModel = new PlanetCreateViewModel
            {
                Name = "",
                Mass = -1,
                Radius = 0,
                DistanceFromEarth = -100,
                ImageUrl = "invalid-url",
                GalaxyId = 1,
                StarId = 1

            };
            await service.CreatePlanetAsync(invalidModel, "user123");

            var count = context.Planets.Count();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CreatePlanetShouldAddToDatabaseWhenModelIsValid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("ValidPlanetDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var galaxy = new Galaxy { Name = "Galaxy", Description = "Test galaxy" };
            var star = new Star { Name = "Star", Description = "Test star", Galaxy = galaxy };
            context.Galaxies.Add(galaxy);
            context.Stars.Add(star);
            await context.SaveChangesAsync();



            var mapper = GetMapper();

            var service = new PlanetService(context, mapper);

            var model = new PlanetCreateViewModel
            {
                Name = "Earth",
                Description = "Blue planet",
                Mass = 1,
                Radius = 1,
                DistanceFromEarth = 10,
                ImageUrl = "https://earth.com/img.png",
                GalaxyId = galaxy.Id,
                StarId = star.Id
            };

            await service.CreatePlanetAsync(model, "user-123");

            var count = context.Planets.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnAllPlanets()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetAllPlanetsDb")
                .Options;

            using var context = new ApplicationDbContext(options);

            var galaxy = new Galaxy
            {
                Name = "Milky Way",
                Description = "Our home galaxy"
            };
            var star = new Star
            { 
                Name = "Sun",
                Description = "Our star",
                Galaxy = galaxy
            };

            context.Galaxies.Add(galaxy);
            context.Stars.Add(star);
            await context.SaveChangesAsync();

            context.Planets.AddRange
            (
                 new Planet
                 {
                     Name = "Mars",
                     Description = "Red",
                     Mass = 1,
                     Radius = 1,
                     DistanceFromEarth = 100,
                     ImageUrl = "https://img.com",
                     GalaxyId = galaxy.Id,
                     StarId = star.Id,
                     CreatorId = "user-1",
                     DiscoveredOn = DateTime.UtcNow,
                     DiscoveredAgo = "Unknown"
                 },
                 new Planet
                 {
                     Name = "Venus",
                     Description = "Hot",
                     Mass = 1,
                     Radius = 1,
                     DistanceFromEarth = 150,
                     ImageUrl = "https://img.com",
                     GalaxyId = galaxy.Id,
                     StarId = star.Id,
                     CreatorId = "user-1",
                     DiscoveredOn = DateTime.UtcNow,
                     DiscoveredAgo = "Unknown"
                 }
            );
            await context.SaveChangesAsync();

            var mapper = GetMapper();
            var service = new PlanetService(context, mapper);

            var planets = await service.GetAllAsync();

            Assert.Equal(2, planets.Count());
            Assert.Contains(planets, p => p.Name == "Mars");
            Assert.Contains(planets, p => p.Name == "Venus");
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectPlanet()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetPlanetByIdDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var galaxy = new Galaxy
            {
                Name = "Milky Way",
                Description = "Our galaxy"
            };

            var star = new Star
            {
                Name = "Sun",
                Description = "Our star",
                Galaxy = galaxy
            };

            context.Galaxies.Add(galaxy);
            context.Stars.Add(star);
            await context.SaveChangesAsync();

            var planet = new Planet
            {
                Name = "Jupiter",
                Description = "Gas giant",
                Mass = 100,
                Radius = 200,
                DistanceFromEarth = 500,
                ImageUrl = "https://jupiter.com",
                GalaxyId = galaxy.Id,
                StarId = star.Id
            };
            context.Planets.Add(planet);
            await context.SaveChangesAsync();

            var mapper = GetMapper();

            var service = new PlanetService(context, mapper);

            var result = await service.GetByIdAsync(planet.Id);

            Assert.NotNull(result);
            Assert.Equal("Jupiter", result.Name);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullWhenPlanetDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("NullPlanetDb")
                .Options;

            using var context = new ApplicationDbContext(options);

            var mapper = GetMapper();
            var service = new PlanetService(context, mapper);

            var result = await service.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePlanetAsyncShouldUpdateFields()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UpdatePlanetDb")
                .Options;

            using var context = new ApplicationDbContext(options);

            var planet = new Planet
            {
                Name = "Temp",
                Description = "Test",
                Mass = 1,
                Radius = 2,
                DistanceFromEarth = 300,
                ImageUrl = "https://old.com",
                GalaxyId = 1,
                StarId = 1
            };
            context.Planets.Add(planet);
            await context.SaveChangesAsync();

            var mapper = GetMapper();
            var service = new PlanetService(context, mapper);

            var updated = new PlanetCreateViewModel
            {
                Name = "Updated",
                Description = "Updated desc",
                Mass = 2,
                Radius = 3,
                DistanceFromEarth = 500,
                ImageUrl = "https://new.com",
                GalaxyId = 2,
                StarId = 2
            };

            await service.UpdatePlanetAsync(planet.Id, updated);

            var result = await context.Planets.FindAsync(planet.Id);
            Assert.Equal("Updated", result.Name);
            Assert.Equal("Updated desc", result.Description);
            Assert.Equal(2, result.Mass);
            Assert.Equal(3, result.Radius);
            Assert.Equal("https://new.com", result.ImageUrl);
        }
        [Fact]
        public async Task DeletePlanetAsyncShouldRemovePlanet()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DeletePlanetDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var planet = new Planet
            {
                Name = "ToDelete",
                Description = "Temporary",
                Mass = 1,
                Radius = 1,
                DistanceFromEarth = 100,
                ImageUrl = "https://delete.com",
                GalaxyId = 1,
                StarId = 1
            };
            context.Planets.Add(planet);
            await context.SaveChangesAsync();

            var mapper = GetMapper();
            var service = new PlanetService(context, mapper);
            await service.DeletePlanetAsync(planet.Id);

            var result = await context.Planets.FindAsync(planet.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeletePlanetAsyncShouldDoNothingWhenPlanetNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DeleteMissingPlanetDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mapper = GetMapper();
            var service = new PlanetService(context, mapper);

            await service.DeletePlanetAsync(999);
            Assert.Empty(context.Planets);
        }


    }
}
