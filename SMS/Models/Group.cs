namespace SMS.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
