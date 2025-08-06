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
using AutoMapper;

namespace AstroFrameWeb.Services.Implementations
{
    public class PlanetService : IPlanetService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PlanetService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PlanetViewModel>> GetAllAsync()
        {
            var planets = await _context.Planets
                .Include(p => p.Galaxy)
                .Include(p => p.Star)
                .ToListAsync(); 

            return _mapper.Map<IEnumerable<PlanetViewModel>>(planets);
        }

        public async Task<PlanetViewModel?> GetByIdAsync(int id)
        {
            var planet =  await _context.Planets
                .Include(p => p.Galaxy)
                .Include(p => p.Star)
                .FirstOrDefaultAsync(p => p.Id == id);

            return planet == null ? null : _mapper.Map<PlanetViewModel>(planet);
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

            var planet = _mapper.Map<Planet>(model);
            planet.CreatorId = creatorId;
            planet.DiscoveredOn = DateTime.UtcNow;
            planet.DiscoveredAgo = "Unknown";

            _context.Planets.Add(planet);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePlanetAsync(int id, PlanetCreateViewModel model)
        {
            var planet = await _context.Planets.FindAsync(id);
            if (planet == null) return;
            _mapper.Map(model, planet);//
            //planet.Name = model.Name;
            //planet.Description = model.Description;
            //planet.Mass = model.Mass;
            //planet.Radius = model.Radius;
            //planet.DistanceFromEarth = model.DistanceFromEarth;
            //planet.ImageUrl = model.ImageUrl;
            //planet.GalaxyId = model.GalaxyId;
            //planet.StarId = model.StarId;

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
