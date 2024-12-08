using demobkend.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace demobkend.Models
{
    public class LearningPath
    {
        [Key]
        public int PathId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public List<Course> Courses { get; set; }  

        public string CompletionCriteria { get; set; }  // e.g. complete all courses

        public bool CertificationAwarded { get; set; }
    }



}
