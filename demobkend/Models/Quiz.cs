using demobkend.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public int TotalQuestions { get; set; }

        public TimeSpan Duration { get; set; }

        public int PassingScore { get; set; }
    }



}
