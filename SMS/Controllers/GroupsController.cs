using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Models;

namespace SMS.Controllers
{
    public class GroupsController : Controller
    {
        private readonly SMSContext _context;
        private readonly UserManager<SMSUser> _userManager;


        public GroupsController(SMSContext context, UserManager<SMSUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context.Groups.Include(g => g.Subject);
            return View(await context.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        public IActionResult Create()
        {
            var subjects = _context.Subjects.ToList();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedBy,SubjectId")] Group @group)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            SMSUser user = await _userManager.FindByIdAsync(userId);

            var newGroup = new Group
            {
                Name = @group.Name,
                Description = @group.Description,
                CreatedBy = user.FirstName,
                SubjectId = @group.SubjectId
            };

            _context.Add(newGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View(@group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SubjectId")] Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Subject");
            ModelState.Remove("CreatedBy");
            if (ModelState.IsValid)
            {
                try
                {
                    var existingGroup = await _context.Groups.FindAsync(id);
                    group.CreatedBy = existingGroup.CreatedBy;

                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.Id))
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

            return View(group);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Groups'  is null.");
            }
            var @group = await _context.Groups.FindAsync(id);
            if (@group != null)
            {
                _context.Groups.Remove(@group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
