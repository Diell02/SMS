namespace SMS.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int GradeS { get; set; }
    }
}
