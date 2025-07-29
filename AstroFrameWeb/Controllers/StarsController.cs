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
using AstroFrameWeb.Data.Models.ViewModels;

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
        public IActionResult Index(int index = 0 , string? searchStr = null)
        {
            var starsQuery = _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.StarType)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchStr))
            {
                starsQuery = starsQuery
                    .Where(s => s.Name.Contains(searchStr));
                index = 0;
            }
            var stars = starsQuery.ToList();

            if (!stars.Any())
            {
                 return View("Empty");
            }
            index = Math.Clamp(index, 0, stars.Count - 1);
            var currentStar = stars[index];

            ViewBag.Index = index;
            ViewBag.Total = stars.Count;

            return View(currentStar);

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
                .Include(s => s.StarType)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (star == null)
            {
                return NotFound();
            }

            return View(star);
        }

        // GET: Stars/Create
        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new StarCreateViewModel
            {
                Galaxies = _context.Galaxies
            .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name }),
                StarTypes = _context.StarTypes
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
            };
            return View(viewModel);
        }

        // POST: Stars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize] // samo lognati da syzdavat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StarCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Galaxies = _context.Galaxies
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name });
                model.StarTypes = _context.StarTypes
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });
                return View(model);
            }
            var star = new Star
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                GalaxyId = model.GalaxyId,
                StarTypeId = model.StarTypeId,
                ImageUrl = model.ImageUrl,
                IsPurchased = false,
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedOn = DateTime.UtcNow,
                DiscoveredAgo = "500 years ago"
            };
            _context.Stars.Add(star);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = star.Id });
        }

        // GET: Stars/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {

            var star = await _context.Stars.FindAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (star == null || (star.OwnerId != userId && !User.IsInRole("Admin")))
            {
                return Forbid();
            }
            if (id == null)
            {
                return NotFound();
            }
 
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

        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,IsPurchased,CreatedOn,OwnerId,GalaxyId,StarTypeId,ImageUrl")] Star star)
        {
            if (id != star.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                if (ViewData["GalaxyId"] == null)
                    ViewData["GalaxyId"] = new SelectList(_context.Galaxies, "Id", "Name", star.GalaxyId);

                if (ViewData["StarTypeId"] == null)
                    ViewData["StarTypeId"] = new SelectList(_context.StarTypes, "Id", "Name", star.StarTypeId);

                return View(star);
            }
            try

            {
                _context.Update(star);
                await _context.SaveChangesAsync();

           }
            catch (DbUpdateConcurrencyException)
            {
                if (!StarExists(star.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));

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
                .FirstOrDefaultAsync(s => s.Id == id);
            if (star == null)
            {
                return NotFound();
            }

            return View(star);
        }

        // POST: Stars/Delete/5
        [Authorize(Roles = "Admin")]
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

        public IActionResult ViewStar(int index = 0)
        {
            var stars = _context.Stars
               .Include(s => s.Galaxy)
               .Include(s => s.StarType)
               .ToList();

            if (!stars.Any())
                return NotFound("No stars found.");

            index = Math.Clamp(index, 0, stars.Count - 1);

            var currentStar = stars[index];

            ViewBag.Index = index;
            ViewBag.Total = stars.Count;

            return View(currentStar);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            var star = await _context.Stars.FindAsync(id);

            if (star == null)
                return NotFound();

            if (star.IsPurchased)
            {
                TempData["BuyMessage"] = "Star already purchased.";
                return RedirectToAction("Index");

            }
            star.IsPurchased = true;
            star.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        //helper
        private void PopulateDropDownsHelper(int? galaxyId = null, int? starTypeId = null)
        {
           var galaxies = _context.Galaxies.AsNoTracking().ToList();
           var starTypes = _context.StarTypes.AsNoTracking().ToList();
            ViewBag.Galaxies = new SelectList(_context.Galaxies.AsNoTracking(), "Id", "Name", galaxyId);
            ViewBag.StarTypes = new SelectList(_context.StarTypes.AsNoTracking(), "Id", "Name", starTypeId);
        }
    }
}
