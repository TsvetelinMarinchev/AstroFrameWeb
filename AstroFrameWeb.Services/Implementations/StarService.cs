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
    public class StarService : IStarService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StarService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public  async Task CreateStarAsync(StarCreateViewModel model, string creatorId)
        {
            if (string.IsNullOrWhiteSpace(model.Name) ||
        string.IsNullOrWhiteSpace(model.Description) ||
        model.Price <= 0 ||
        model.GalaxyId <= 0 ||
        model.StarTypeId <= 0 ||
        !Uri.IsWellFormedUriString(model.ImageUrl, UriKind.Absolute))
            {
                return;
            }

            var star = _mapper.Map<Star>(model);
            star.OwnerId = creatorId;
            star.CreatedOn = DateTime.UtcNow;
            star.DiscoveredAgo = "Unknown";

            _context.Stars.Add(star);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStarAsync(int id)
        {
            var star = await _context.Stars.FindAsync(id);
            if (star == null) return;

            _context.Stars.Remove(star);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StarViewModel>> GetAllAsync()
        {
            var stars = await _context.Stars
                   .Include(s => s.Galaxy)
                   .Include(s => s.StarType)
                   .ToListAsync();

            return _mapper.Map<IEnumerable<StarViewModel>>(stars);
        }

         public async Task<StarViewModel?> GetByIdAsync(int id)
        {
            var star = await _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.StarType)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (star == null) return null;

            return _mapper.Map<StarViewModel>(star);
        }

        public async Task UpdateStarAsync(int id, StarCreateViewModel model)
        {
            var star = await _context.Stars.FindAsync(id);
            if (star == null) return;

            _mapper.Map(model, star);

            await _context.SaveChangesAsync();
        }
    }
}
