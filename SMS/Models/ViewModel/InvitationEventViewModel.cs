using Microsoft.AspNetCore.Identity;
using SMS.Data;
using SMS.Enums;

namespace SMS.Models.ViewModel
{
    public class InvitationEventViewModel
    {
        public IEnumerable<SMSUser> Users { get; set; }
        public IEnumerable<Invitation> Invitations { get; set; }
        public UserManager<SMSUser> UserManager { get; set; }
        public SMSContext Context { get; set; }
        public Approval Status { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; }
    }
}
