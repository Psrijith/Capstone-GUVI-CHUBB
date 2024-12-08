using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class InstructorDashboard
    {
        [Key]
        public int DashboardId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public List<int> CourseIds { get; set; }  // Courses managed by the instructor

        public decimal TotalEarnings { get; set; }

        public List<string> Messages { get; set; }
    }



}
