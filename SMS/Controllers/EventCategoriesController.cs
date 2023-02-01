using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Models;

namespace SMS.Controllers
{
    public class EventCategoriesController : Controller
    {
        private readonly SMSContext _context;

        public EventCategoriesController(SMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCategories.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EventCategories == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] EventCategory eventCategory)
        {
            ModelState.Remove("Events");
            if (ModelState.IsValid)
            {
                _context.Add(eventCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EventCategories == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategories.FindAsync(id);
            if (eventCategory == null)
            {
                return NotFound();
            }
            return View(eventCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] EventCategory eventCategory)
        {
            if (id != eventCategory.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Events");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCategoryExists(eventCategory.Id))
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
            return View(eventCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EventCategories == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EventCategories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EventCategories'  is null.");
            }
            var eventCategory = await _context.EventCategories.FindAsync(id);
            if (eventCategory != null)
            {
                _context.EventCategories.Remove(eventCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCategoryExists(int id)
        {
            return _context.EventCategories.Any(e => e.Id == id);
        }
    }
}
