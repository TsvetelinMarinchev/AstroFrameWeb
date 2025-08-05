using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
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
            var isAdmin = User.IsInRole("Admin");


            IQueryable<Star> starsQuery = _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.StarType);

            IQueryable<Galaxy> galaxiesQuery = _context.Galaxies.AsQueryable();

            IQueryable<Planet> planetsQuery = _context.Planets
                .Include(p => p.Star)
                .Include(p => p.Galaxy);

            if (!isAdmin)
            {
                starsQuery = starsQuery.Where(s => s.OwnerId == userId);
                galaxiesQuery = galaxiesQuery.Where(g => g.CreatorId == userId);
                planetsQuery = planetsQuery.Where(p => p.CreatorId == userId);
            }

            var stars = await starsQuery.ToListAsync();
            var galaxies = await galaxiesQuery.ToListAsync();
            var planets = await planetsQuery.ToListAsync();

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
