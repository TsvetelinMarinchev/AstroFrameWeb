using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.Services.Interfaces;
using AstroFrameWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Services.Implementations
{
    public class PlanetService : IPlanetService
    {
        private readonly ApplicationDbContext _context;
        public PlanetService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PlanetViewModel>> GetAllAsync()
        {
            return await _context.Planets
                .Include(p => p.Galaxy)
                .Include(p => p.Star)
                .Select(p => new PlanetViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Mass = p.Mass,
                    Radius = p.Radius,
                    DistanceFromEarth = p.DistanceFromEarth,
                    GalaxyName = p.Galaxy != null ? p.Galaxy.Name : "Unknown",
                    StarName = p.Star != null ? p.Star.Name : "Unknown",
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<Planet?> GetByIdAsync(int id)
        {
            return await _context.Planets
                .Include(p => p.Galaxy)
                .Include(p => p.Star)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePlanetAsync(PlanetCreateViewModel model, string creatorId)
        {
            if (string.IsNullOrWhiteSpace(model.Name) ||
                model.Mass <= 0 ||
                model.Radius <= 0 ||
                model.DistanceFromEarth <= 0 ||
                !Uri.IsWellFormedUriString(model.ImageUrl, UriKind.Absolute))
            {
                return;
            }

            var planet = new Planet
            {
                Name = model.Name,
                Description = model.Description,
                Mass = model.Mass,
                Radius = model.Radius,
                DistanceFromEarth = model.DistanceFromEarth,
                ImageUrl = model.ImageUrl,
                GalaxyId = model.GalaxyId,
                StarId = model.StarId,
                CreatorId = creatorId,
                DiscoveredOn = DateTime.UtcNow,
                DiscoveredAgo = "Unknown"
            };

            _context.Planets.Add(planet);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePlanetAsync(int id, PlanetCreateViewModel model)
        {
            var planet = await _context.Planets.FindAsync(id);
            if (planet == null) return;

            planet.Name = model.Name;
            planet.Description = model.Description;
            planet.Mass = model.Mass;
            planet.Radius = model.Radius;
            planet.DistanceFromEarth = model.DistanceFromEarth;
            planet.ImageUrl = model.ImageUrl;
            planet.GalaxyId = model.GalaxyId;
            planet.StarId = model.StarId;

            await _context.SaveChangesAsync();
        }
        public async Task DeletePlanetAsync(int id)
        {
            var planet = await _context.Planets.FindAsync(id);
            if (planet == null) return;

            _context.Planets.Remove(planet);
            await _context.SaveChangesAsync();
        }
    }

}
