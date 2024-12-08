using demobkend.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class CourseLearningPath
    {
        [Key]
        public int CourseLearningPathId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [ForeignKey("LearningPath")]
        public int PathId { get; set; }
        public LearningPath LearningPath { get; set; }
    }

}
