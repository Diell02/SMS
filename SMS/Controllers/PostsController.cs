using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Models;
using SMS.Validators;

namespace SMS.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly SMSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostsController(SMSContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int groupId)
        {
            var posts = await _context.Posts
                .Where(p => p.GroupId == groupId)
                .ToListAsync();

            return View(posts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        private SMSUser GetApplicationUser(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            return user;
        }

        public IActionResult Create(int groupId)
        {
            var post = new Post { GroupId = groupId };
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedBy,GroupId")] Post post)
        {
            ModelState.Remove("CreatedBy");
            ModelState.Remove("Group");
            if (ModelState.IsValid)
            {
                var userAccessor = _httpContextAccessor.HttpContext.User;
                post.CreatedBy = GetApplicationUser(userAccessor);
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Groups", new { id = post.GroupId });
            }
            return RedirectToAction("Details", "Groups", new { id = post.GroupId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,GroupId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            var validator = new PostValidator();
            ValidationResult result = validator.Validate(post);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }
            try
            {
                var postToUpdate = await _context.Posts.FindAsync(id);
                postToUpdate.Title = post.Title;
                postToUpdate.Description = post.Description;
                _context.Update(postToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "Groups", new { id = post.GroupId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'SMSContext.Post'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Groups", new { id = post.GroupId });
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
