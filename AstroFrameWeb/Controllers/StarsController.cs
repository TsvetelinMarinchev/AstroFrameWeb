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
using System.Security.Claims;

namespace AstroFrameWeb.Controllers
{
    public class StarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stars
        public async Task<IActionResult> Index(string searchStr)
        {
            var starQuery = _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.Owner)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStr))
            {
                starQuery = starQuery
                    .Where(s => s.Name.Contains(searchStr));

            }

            //var applicationDbContext = _context.Stars.Include(s => s.Galaxy).Include(s => s.Owner);
            return View(await starQuery.ToListAsync());
        }

        // GET: Stars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var star = await _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (star == null)
            {
                return NotFound();
            }

            return View(star);
        }

        // GET: Stars/Create
        public IActionResult Create()
        {
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Description");
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
           // ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Name");
            return View();
        }

        // POST: Stars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,IsPurchased,GalaxyId")] Star star)
        {
            if (ModelState.IsValid)
            {
                star.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                star.CreatedOn = DateTime.UtcNow;
                _context.Add(star);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Name", star.GalaxyId);
            return View(star);
        }

        // GET: Stars/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var star = await _context.Stars.FindAsync(id);
            if (star == null)
            {
                return NotFound();
            }
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Description", star.GalaxyId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", star.OwnerId);
            return View(star);
        }

        // POST: Stars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,IsPurchased,CreatedOn,OwnerId,GalaxyId")] Star star)
        {
            if (id != star.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(star);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StarExists(star.Id))
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
            ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Description", star.GalaxyId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", star.OwnerId);
            return View(star);
        }

        // GET: Stars/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var star = await _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (star == null)
            {
                return NotFound();
            }

            return View(star);
        }

        // POST: Stars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var star = await _context.Stars.FindAsync(id);
            if (star != null)
            {
                _context.Stars.Remove(star);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StarExists(int id)
        {
            return _context.Stars.Any(e => e.Id == id);
        }
    }
}
