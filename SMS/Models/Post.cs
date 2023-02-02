namespace SMS.Models
{
    public class Post
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SMSUser CreatedBy { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}