namespace SMS.Models
{
    public class EventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Event> Events { get; set; }
    }
}
