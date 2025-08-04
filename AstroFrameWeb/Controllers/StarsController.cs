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
        public async Task<IActionResult> Index(int index = 0, string? searchStr = null)
        {
            var starsQuery = _context.Stars
                .Include(s => s.Galaxy)
                .Include(s => s.StarType)
                .Include(s => s.Owner)
                .AsNoTracking();
            if (!string.IsNullOrEmpty(searchStr))
            {
                starsQuery = starsQuery
                    .Where(s => s.Name.Contains(searchStr));
                index = 0;
            }
            var stars = await starsQuery.ToListAsync();

            if (!stars.Any())
            {
                return View("Empty");
            }
            index = Math.Clamp(index, 0, stars.Count - 1);
            var currentStar = stars[index];

            ViewBag.Index = index;
            ViewBag.Total = stars.Count;
            ViewBag.SearchStr = searchStr;

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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && star.OwnerId != userId)
            {
                return Forbid();
            }

            PopulateDropDownsHelper(star.GalaxyId, star.StarTypeId);
            return View(star);
        }

        // POST: Stars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,GalaxyId,StarTypeId,ImageUrl,OwnerId")] Star updated)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var err in errors)
                {
                    Console.WriteLine("EDIT ERROR: " + err);
                }

                PopulateDropDownsHelper(updated.GalaxyId, updated.StarTypeId);
                return View(updated);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isAdmin = User.IsInRole("Admin");

            var star = await _context.Stars.FindAsync(id);
            if (star == null || (!isAdmin && star.OwnerId != userId))
                return Forbid();

            star.Name = updated.Name;
            star.Description = updated.Description;
            star.Price = updated.Price;
            star.GalaxyId = updated.GalaxyId;
            star.StarTypeId = updated.StarTypeId;
            star.ImageUrl = updated.ImageUrl;

            _context.Entry(star).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = star.Id });

        }

        // GET: Stars/Delete/5
        [Authorize]
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && star.OwnerId != userId) return Forbid();

            return View(star);
        }

        // POST: Stars/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var star = await _context.Stars.FindAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isAdmin = User.IsInRole("Admin");

            if (star == null || (!isAdmin && star.OwnerId != userId)) return Forbid();

            _context.Stars.Remove(star);

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

        //my stars
        //[Authorize]
        //public IActionResult MyStars()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var stars = _context.Stars
        //        .Where(s => s.OwnerId == userId)
        //        .Include(s => s.Galaxy)
        //        .Include(s => s.StarType)
        //        .ToList();

        //    return View("MyStars", stars);
        //}

        //helper
        private void PopulateDropDownsHelper(int? galaxyId = null, int? starTypeId = null)
        {

            //var galaxies = _context.Galaxies.AsNoTracking().ToList();
            //var starTypes = _context.StarTypes.AsNoTracking().ToList();
            //ViewBag.Galaxies = new SelectList(_context.Galaxies.AsNoTracking(), "Id", "Name", galaxyId);
            //ViewBag.StarTypes = new SelectList(_context.StarTypes.AsNoTracking(), "Id", "Name", starTypeId);
            ViewBag.Galaxies = new SelectList(
            _context.Galaxies.AsNoTracking().ToList(),"Id", "Name", galaxyId);

            ViewBag.StarTypes = new SelectList(
                _context.StarTypes.AsNoTracking().ToList(),"Id", "Name", starTypeId);
        }
    }
}
