using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class QuizResult
    {
        [Key]
        public int ResultId { get; set; }

        [Required]
        public int QuizId { get; set; }

        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public double Score { get; set; }

        [StringLength(500)]
        public string Feedback { get; set; }

        public DateTime DateCompleted { get; set; }
    }

}
