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
    public class StarService : IStarService
    {
        private readonly ApplicationDbContext _context;
        public StarService(ApplicationDbContext context)
        {
            _context = context;
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

            var star = new Star
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                GalaxyId = model.GalaxyId,
                StarTypeId = model.StarTypeId,
                ImageUrl = model.ImageUrl,
                DiscoveredAgo = "Unknown",
                CreatedOn = DateTime.UtcNow,
                OwnerId = creatorId
            };

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
            return await _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.StarType)
                .Select(s => new StarViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    GalaxyName = s.Galaxy != null ? s.Galaxy.Name : "Unknown",
                    StarTypeName = s.StarType != null ? s.StarType.Name : "Unknown",
                    ImageUrl = s.ImageUrl,
                    DiscoveredAgo = s.DiscoveredAgo ?? "Unknown"
                })
                .ToListAsync();
        }

         public async Task<StarViewModel?> GetByIdAsync(int id)
        {
            var star = await _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.StarType)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (star == null) return null;

            return new StarViewModel
            {
                Id = star.Id,
                Name = star.Name,
                Description = star.Description,
                Price = star.Price,
                GalaxyName = star.Galaxy?.Name ?? "Unknown",
                StarTypeName = star.StarType?.Name ?? "Unknown",
                ImageUrl = star.ImageUrl,
                DiscoveredAgo = star.DiscoveredAgo ?? "Unknown"
            };
        }

        public async Task UpdateStarAsync(int id, StarCreateViewModel model)
        {
            var star = await _context.Stars.FindAsync(id);
            if (star == null) return;

            star.Name = model.Name;
            star.Description = model.Description;
            star.Price = model.Price;
            star.ImageUrl = model.ImageUrl;
            star.GalaxyId = model.GalaxyId;
            star.StarTypeId = model.StarTypeId;

            await _context.SaveChangesAsync();
        }
    }
}
