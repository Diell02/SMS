using SMS.Enums;

namespace SMS.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public string? Username { get; set; }
        public string? CreatedBy { get; set; }
        //public Approval Status { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
