namespace demobkend.DTOs
{
    public class CourseDTO
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int InstructorId { get; set; }
        public InstructorDTO Instructor { get; set; }
        public string Category { get; set; }
        public string DifficultyLevel { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }
    }
}
