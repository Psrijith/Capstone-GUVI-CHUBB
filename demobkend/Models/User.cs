using System.ComponentModel.DataAnnotations;

namespace demobkend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }  // Role can be Learner, Instructor, Admin, Support

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }



}
