using demobkend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace demobkend.models
{
    public class Course 
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }

        // This is the missing navigation property
        public User Instructor { get; set; }  // Navigation property for the related instructor (User)

        public string Category { get; set; }  // e.g. Web Development, Data Science, etc.

        public string DifficultyLevel { get; set; }  // e.g. Beginner, Intermediate, Advanced

        public TimeSpan Duration { get; set; }

        public string Status { get; set; }  // e.g. Active, Inactive 

        [JsonIgnore]
        public List<LearningPath> LearningPaths { get; set; }
    }


}