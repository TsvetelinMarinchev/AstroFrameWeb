using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace AstroFrameWeb.Services.Implementations
{
    public class GalaxyService : IGalaxyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GalaxyService(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            _mapper = mapper;
        }
        public async Task CreateGalaxyAsync(GalaxyCreateViewModel model, string creatorId)
        {
            if (string.IsNullOrWhiteSpace(model.Name)
                       || model.NumberOfStars <= 0
                       || model.DistanceFromEarth <= 0
                       || !Uri.IsWellFormedUriString(model.ImageUrl, UriKind.Absolute))//dali e validen Url
            {
                return;
            }
            var galaxy = _mapper.Map<Galaxy>(model);
            galaxy.CreatorId = creatorId;
            
            _context.Galaxies.Add(galaxy);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Galaxy>> GetAllAsync()
        {
            return await _context.Galaxies.ToListAsync();
        }
        public async Task<Galaxy?> GetByIdAsync(int id)
        {
            return await _context.Galaxies
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task UpdateGalaxyAsync(int id, GalaxyCreateViewModel model)
        {
            var galaxy = await _context.Galaxies.FindAsync(id);

            //if (galaxy == null)
            //    return;

            //galaxy.Name = model.Name;
            //galaxy.Description = model.Description;
            //galaxy.GalaxyType = model.GalaxyType;
            //galaxy.NumberOfStars = model.NumberOfStars;
            //galaxy.DistanceFromEarth = model.DistanceFromEarth;
            //galaxy.ImageUrl = model.ImageUrl;

            //_context.Galaxies.Update(galaxy);
            _mapper.Map(model, galaxy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGalaxyAsync(int id)
        {
            var galaxy = await _context.Galaxies.FindAsync(id);

            if (galaxy == null) return;
            _context.Galaxies.Remove(galaxy);

            await _context.SaveChangesAsync();
        }
    }
}
