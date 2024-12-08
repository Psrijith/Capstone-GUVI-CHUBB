using demobkend.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class CourseContent
    {
        [Key]
        public int ContentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public string ContentType { get; set; }  // e.g. Video, PDF, Interactive Activity

        public string FileUrl { get; set; }

        public string ContentDescription { get; set; }

        public int ContentOrder { get; set; }  // Order of content in the course
    }



}
