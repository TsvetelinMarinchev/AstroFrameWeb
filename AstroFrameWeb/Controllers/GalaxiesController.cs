using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
using Microsoft.AspNetCore.Authorization;
using AstroFrameWeb.Data.Models.ViewModels;
using System.Security.Claims;
using AstroFrameWeb.Data.Enums;

namespace AstroFrameWeb.Controllers
{
    public class GalaxiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalaxiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Galaxies
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Galaxies.Include(g => g.Creator);
        //    return View(await applicationDbContext.ToListAsync());
        //}
        [AllowAnonymous]
        public IActionResult Index(string searchStr, int index = 0)
        {
            var galaxies = _context.Galaxies
                  .Include(g => g.Creator)
                  .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                galaxies = galaxies.Where(g => g.Name.ToLower().Contains(searchStr.ToLower()));
            }
            var galaxyList = galaxies.ToList();

            if (!galaxies.Any())
                return NotFound("No galaxies found.");

            if (index < 0) index = 0;
            if (index >= galaxyList.Count) index = galaxyList.Count - 1;

            var current = galaxyList[index];
            ViewBag.Index = index;
            ViewBag.Total = galaxyList.Count;
            ViewBag.SearchStr = searchStr;

            return View(current);
        }

        // GET: Galaxies/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galaxy = await _context.Galaxies
                .Include(g => g.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galaxy == null)
            {
                return NotFound();
            }

            return View(galaxy);
        }

        // GET: Galaxies/Create
        [Authorize]
        public IActionResult Create()
        {

            var galaxyTypes = Enum.GetValues(typeof(GalaxyType))
                             .Cast<GalaxyType>()
                             .Select(g => new SelectListItem
                             {
                                 Value = ((int)g).ToString(),
                                 Text = g.ToString()
                             }).ToList();

            ViewBag.GalaxyTypes = galaxyTypes;

            return View(new GalaxyCreateViewModel());
        }

        // POST: Galaxies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(GalaxyCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var galaxy = new Galaxy
            {
                Name = model.Name,
                Description = model.Description,
                NumberOfStars = model.NumberOfStars,
                DistanceFromEarth = model.DistanceFromEarth,
                GalaxyType = model.GalaxyType,
                ImageUrl = model.ImageUrl,
                DiscoveredOn = DateTime.UtcNow,
                DiscoveredAgo = "Billion years ago",
                CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            _context.Galaxies.Add(galaxy);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Galaxies/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galaxy = await _context.Galaxies.FindAsync(id);
            if (galaxy == null)
            {
                return NotFound();
            }
            //user
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && galaxy.CreatorId != currentUserId)
                return Forbid();

            ViewBag.GalaxyTypes = Enum.GetValues(typeof(GalaxyType))
                            .Cast<GalaxyType>()
                            .Select(g => new SelectListItem
                            {
                                Value = ((int)g).ToString(),
                                Text = g.ToString()
                            });
            return View(galaxy);
        }

        // POST: Galaxies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,GalaxyType,NumberOfStars,DistanceFromEarth,DiscoveredOn,ImageUrl,CreatorId")] Galaxy updated)
        {
            var galaxy = await _context.Galaxies.FindAsync(id);
            if (galaxy == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && galaxy.CreatorId != userId)
                return Forbid();
            if (!ModelState.IsValid)
            {
                ViewBag.GalaxyTypes = Enum.GetValues(typeof(GalaxyType))
                    .Cast<GalaxyType>()
                    .Select(g => new SelectListItem
                    {
                        Value = ((int)g).ToString(),
                        Text = g.ToString()
                    });

                return View(updated);
            }
            galaxy.Name = updated.Name;
            galaxy.Description = updated.Description;
            galaxy.GalaxyType = updated.GalaxyType;
            galaxy.NumberOfStars = updated.NumberOfStars;
            galaxy.DistanceFromEarth = updated.DistanceFromEarth;
            galaxy.DiscoveredOn = updated.DiscoveredOn;
            galaxy.ImageUrl = updated.ImageUrl;

            _context.Entry(galaxy).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = galaxy.Id });

        }

        // GET: Galaxies/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galaxy = await _context.Galaxies
                .Include(g => g.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galaxy == null)
                return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && galaxy.CreatorId != currentUserId)
                return Forbid();

            return View(galaxy);
        }

        // POST: Galaxies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galaxy = await _context.Galaxies.FindAsync(id);
            if (galaxy == null)
            {
                return NotFound();
            }


            var currentUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && galaxy.CreatorId != currentUserId)
                return Forbid();

            _context.Galaxies.Remove(galaxy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalaxyExists(int id)
        {
            return _context.Galaxies.Any(e => e.Id == id);
        }
    }
}
