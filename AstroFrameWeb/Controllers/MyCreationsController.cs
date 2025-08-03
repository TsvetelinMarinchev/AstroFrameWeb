using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AstroFrameWeb.Controllers
{
    [Authorize]
    public class MyCreationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyCreationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public  async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var stars = await _context.Stars
                .Where(s => s.OwnerId == userId)
                .Include(s => s.Galaxy)
                .Include(s => s.StarType)
                .ToListAsync();

            var galaxies = await _context.Galaxies
                .Where(g => g.CreatorId == userId)
                .ToListAsync();

            var planets = await _context.Planets
                .Where(p => p.CreatorId == userId)
                .Include(p => p.Star)
            .Include(p => p.Galaxy)
                .ToListAsync();

            var model = new MyCreationViewModel
            {
                Stars = stars,
                Galaxies = galaxies,
                Planets = planets
            };

            return View("Index", model);
        }
    }
    
}
