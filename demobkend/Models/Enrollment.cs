using demobkend.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }  // Navigation property for User

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }  // Navigation property for Course

        public DateTime EnrollmentDate { get; set; }

        public string CompletionStatus { get; set; }

        public double ProgressPercentage { get; set; }
    }
}
