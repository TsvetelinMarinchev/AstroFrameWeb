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

namespace AstroFrameWeb.Controllers
{
    public class PlanetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // TO DO VIEW small size
        public PlanetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Planets
        public IActionResult Index(string searchStr, int index = 0 )
        {
            var planets = _context.Planets
                      .Include(p => p.Star)
                      .Include(p => p.Galaxy)
                      .ToList();

            if (!string.IsNullOrEmpty(searchStr))
            {
                planets = planets
                    .Where(p => p.Name.ToLower().Contains(searchStr.ToLower()))
                    .ToList();
            }

            if (!planets.Any())
                return NotFound("No planets found.");

            if (index < 0) index = 0;
            if (index >= planets.Count) index = planets.Count - 1;

            var current = planets[index];
            ViewBag.Index = index;
            ViewBag.Total = planets.Count;
            ViewBag.SearchStr = searchStr;

            return View("Index", current);
        }
        public IActionResult ViewPlanet(int index = 0)
        {
            var planets = _context.Planets
                .Include(p => p.Star)
                .Include(p => p.Galaxy)
                .ToList();

            if (!planets.Any())
                return NotFound("No planets found.");

            if (index < 0) index = 0;
            if (index >= planets.Count) index = planets.Count - 1;

            var current = planets[index];
            ViewBag.Index = index;
            ViewBag.Total = planets.Count;

            return View("ViewPlanet", current);
        }

        // GET: Planets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planet = await _context.Planets
                .Include(p => p.Creator)
                .Include(p => p.Galaxy)
                .Include(p => p.Star)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        // GET: Planets/Create
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Description");
            ViewData["StarId"] = new SelectList(_context.Stars, "Id", "Description");
            return View();
        }

        // POST: Planets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Mass,Radius,DistanceFromEarth,DiscoveredOn,StarId,GalaxyId,CreatorId")] Planet planet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", planet.CreatorId);
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Description", planet.GalaxyId);
            ViewData["StarId"] = new SelectList(_context.Stars, "Id", "Description", planet.StarId);
            return View(planet);
        }

        // GET: Planets/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planet = await _context.Planets.FindAsync(id);
            if (planet == null)
            {
                return NotFound();
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", planet.CreatorId);
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Description", planet.GalaxyId);
            ViewData["StarId"] = new SelectList(_context.Stars, "Id", "Description", planet.StarId);
            return View(planet);
        }

        // POST: Planets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Mass,Radius,DistanceFromEarth,DiscoveredOn,StarId,GalaxyId,CreatorId")] Planet planet)
        {
            if (id != planet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanetExists(planet.Id))
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
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", planet.CreatorId);
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Description", planet.GalaxyId);
            ViewData["StarId"] = new SelectList(_context.Stars, "Id", "Description", planet.StarId);
            return View(planet);
        }

        // GET: Planets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planet = await _context.Planets
                .Include(p => p.Creator)
                .Include(p => p.Galaxy)
                .Include(p => p.Star)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        // POST: Planets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planet = await _context.Planets.FindAsync(id);
            if (planet != null)
            {
                _context.Planets.Remove(planet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanetExists(int id)
        {
            return _context.Planets.Any(e => e.Id == id);
        }
    }
}
