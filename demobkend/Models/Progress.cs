using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demobkend.Models
{
    public class Progress
    {
        [Key]
        public int ProgressId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("CourseContent")]
        public int ContentId { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletionDate { get; set; }

        public User User { get; set; } // Navigation property
        public CourseContent CourseContent { get; set; } // Navigation property
    }
}
