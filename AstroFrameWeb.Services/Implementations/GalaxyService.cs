using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
using AstroFrameWeb.Data.Models.ViewModels;
using AstroFrameWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroFrameWeb.Services.Implementations
{
    public class GalaxyService : IGalaxyService
    {
        private readonly ApplicationDbContext _context;

        public GalaxyService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task CreateGalaxyAsync(GalaxyCreateViewModel model, string userId)
        {
            var galaxy = new Galaxy
            {
                Name = model.Name,
                Description = model.Description,
                GalaxyType = model.GalaxyType,
                NumberOfStars = model.NumberOfStars,
                DistanceFromEarth = model.DistanceFromEarth,
                ImageUrl = model.ImageUrl,
                DiscoveredOn = DateTime.UtcNow,
                DiscoveredAgo = "Unknown",
                CreatorId = userId
            };
            _context.Galaxies.Add(galaxy);
            await _context.SaveChangesAsync();
        }
    }
}
