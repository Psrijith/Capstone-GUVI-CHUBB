using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        public string Type { get; set; }  // e.g. Course Enrollment, User Activity

        public DateTime GeneratedDate { get; set; }

        public string Data { get; set; }

        [ForeignKey("User")]
        public int CreatedBy { get; set; }
    }



}
