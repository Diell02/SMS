using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Enums;
using SMS.Models;
using SMS.Models.ViewModel;

namespace SMS.Controllers
{
    public class InvitationsController : Controller
    {
        private readonly SMSContext _context;
        private readonly UserManager<SMSUser> _userManager;

        public InvitationsController(SMSContext context, UserManager<SMSUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> SelectUsers(int eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null)
            {
                return NotFound();
            }

            var users = await _context.Users.ToListAsync();
            var invitations = await _context.Invitations.Where(i => i.EventId == eventId).ToListAsync();

            var model = new InvitationEventViewModel
            {
                Users = users,
                Invitations = invitations,
                UserManager = _userManager,
                Context = _context,
                EventId = eventId
            };

            ViewBag.EventCreatedBy = @event.CreatedBy;
            ViewBag.HttpContextUser = HttpContext.User;

            return View(model);
        }

        public async Task<IActionResult> SendInvite(int eventId, string userId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var user = await _context.Users.FindAsync(userId);

            if (@event == null || user == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(HttpContext.User);

            if (currentUserId != @event.CreatedBy)
            {
                return Unauthorized();
            }

            var invitation = new Invitation
            {
                Event = @event,
                User = user,
                Status = Approval.Pending
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SelectUsers), new { eventId });
        }

        public async Task<IActionResult> CancelInvite(int eventId, string userId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var user = await _context.Users.FindAsync(userId);

            if (@event == null || user == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(HttpContext.User);

            if (currentUserId != @event.CreatedBy)
            {
                return Unauthorized();
            }

            var invitation = await _context.Invitations
                .FirstOrDefaultAsync(i => i.EventId == eventId && i.UserId == userId);

            if (invitation == null)
            {
                return NotFound();
            }

            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SelectUsers), new { eventId });
        }

        public async Task<IActionResult> AcceptInvite(int eventId, string userId)
        {
            var invitation = await _context.Invitations.Where(i => i.EventId == eventId && i.UserId == userId).FirstOrDefaultAsync();

            if (invitation == null)
            {
                return NotFound();
            }

            invitation.Status = Approval.Accepted;
            _context.Invitations.Update(invitation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SelectUsers), new { eventId });
        }

        public async Task<IActionResult> DeclineInvite(int eventId, string userId)
        {
            var invitation = await _context.Invitations.Where(i => i.EventId == eventId && i.UserId == userId).FirstOrDefaultAsync();

            if (invitation == null)
            {
                return NotFound();
            }

            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SelectUsers), new { eventId });
        }
    }
}
