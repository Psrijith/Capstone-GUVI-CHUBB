using demobkend.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class Discussion
    {
        [Key]
        public int DiscussionId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public string PostContent { get; set; }

        public DateTime PostDate { get; set; }

        public string Status { get; set; }  // e.g. Open, Closed
    }



}
