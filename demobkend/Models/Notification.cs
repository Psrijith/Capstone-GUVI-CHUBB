using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public string Type { get; set; }  // e.g. New Course, Quiz Result, Assignment Due Date

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }



}
