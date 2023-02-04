namespace SMS.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public EventCategory Category { get; set; }
        public DateTime Created { get; set; } //Meaning when it will be held
        public string CreatedBy { get; set; }
    }
}