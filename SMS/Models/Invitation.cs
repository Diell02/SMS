using SMS.Enums;

namespace SMS.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        public SMSUser User { get; set; }
        public Approval Status { get; set; }
    }
}