namespace demobkend.DTOs
{
    public class CreateCourseRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string DifficultyLevel { get; set; }
        public string Duration { get; set; }  // e.g., "1:30:00" for 1 hour 30 minutes
        public string Status { get; set; }  // e.g., "Active", "Inactive"
    }
}
