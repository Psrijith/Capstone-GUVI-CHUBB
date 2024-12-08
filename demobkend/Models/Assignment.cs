using demobkend.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public string GradingCriteria { get; set; }
    }



}
