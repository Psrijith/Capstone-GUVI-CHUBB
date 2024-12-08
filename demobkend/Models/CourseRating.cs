using demobkend.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class CourseRating
    {
        [Key]
        public int RatingId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public int RatingValue { get; set; }  // e.g. 1 to 5

        public string ReviewComments { get; set; }

        public DateTime RatingDate { get; set; }
    }


}
