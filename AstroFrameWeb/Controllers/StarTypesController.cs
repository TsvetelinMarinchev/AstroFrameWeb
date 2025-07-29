using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AstroFrameWeb.Data;
using AstroFrameWeb.Data.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace AstroFrameWeb.Controllers
{
    public class StarTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StarTypesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.StarTypes.ToListAsync());
        }

        // GET: StarTypes
        public  IActionResult StarTypeView(int? starTypeId, string searchStr, int index = 0)
        {
            var stars = _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.StarType)
                .AsQueryable();

            if (starTypeId.HasValue)
            {
                stars = stars.Where(s => s.StarTypeId == starTypeId);
            }

            if (!string.IsNullOrEmpty(searchStr))
            {
                stars = stars.Where(s => s.Name.ToLower().Contains(searchStr.ToLower()));
            }
            var starList = stars.ToList();

            if (!starList.Any())
                return NotFound("No stars found.");

            if (index < 0) index = 0;
            if (index >= starList.Count) index = starList.Count - 1;

            var current = starList[index];
            ViewBag.Index = index;
            ViewBag.Total = starList.Count;
            ViewBag.SearchStr = searchStr;

            return RedirectToAction("Index", "Stars", new { id = current.Id });

        }

        // GET: StarTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var starType = await _context.StarTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (starType == null)
            {
                return NotFound();
            }

            return View(starType);
        }

        // GET: StarTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StarTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] StarType starType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(starType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(starType);
        }

        // GET: StarTypes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var starType = await _context.StarTypes.FindAsync(id);
            if (starType == null)
            {
                return NotFound();
            }
            return View(starType);
        }

        // POST: StarTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] StarType starType)
        {
            if (id != starType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(starType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StarTypeExists(starType.Id))
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
            return View(starType);
        }

        // GET: StarTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var starType = await _context.StarTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (starType == null)
            {
                return NotFound();
            }

            return View(starType);
        }

        // POST: StarTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var starType = await _context.StarTypes.FindAsync(id);
            if (starType != null)
            {
                _context.StarTypes.Remove(starType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool StarTypeExists(int id)
        {
            return _context.StarTypes.Any(e => e.Id == id);
        }
    }
}
