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
        [Authorize(Roles = "Admin")]
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
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", galaxy.CreatorId);
            ViewData["GalaxyTypes"] = new SelectList(Enum.GetValues(typeof(GalaxyType)));
            return View(new GalaxyCreateViewModel());
        }

        // POST: Galaxies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,GalaxyType,NumberOfStars,DistanceFromEarth,DiscoveredOn,CreatorId")] Galaxy galaxy)
        {
            if (id != galaxy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(galaxy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalaxyExists(galaxy.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", galaxy.CreatorId);
            return View(galaxy);
        }

        // GET: Galaxies/Delete/5
        [Authorize(Roles = "Admin")]
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
            {
                return NotFound();
            }

            return View(galaxy);
        }

        // POST: Galaxies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galaxy = await _context.Galaxies.FindAsync(id);
            if (galaxy != null)
            {
                _context.Galaxies.Remove(galaxy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalaxyExists(int id)
        {
            return _context.Galaxies.Any(e => e.Id == id);
        }
    }
}
