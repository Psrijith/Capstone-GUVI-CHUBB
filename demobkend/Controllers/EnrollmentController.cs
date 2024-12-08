using demobkend.Models;
using demobkend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // Enroll in a course
        [HttpPost("course/{courseId}/user/{userId}")]
        public async Task<IActionResult> EnrollInCourse(int courseId, int userId)
        {
            try
            {
                var enrollment = await _enrollmentService.EnrollInCourseAsync(userId, courseId);
                return CreatedAtAction(nameof(GetEnrollment), new { enrollmentId = enrollment.EnrollmentId }, enrollment);
            }
            catch (Exception ex)
            {
                // If an exception is thrown, it means the course is already enrolled
                return BadRequest(new { message = ex.Message });  // Send the exception message in the response
            }
        }

        // Update progress for a course
        [HttpPut("progress/{courseId}/user/{userId}")]
        public async Task<IActionResult> UpdateProgress(int userId, int courseId, [FromBody] double progressPercentage)
        {
            var enrollment = await _enrollmentService.UpdateProgressAsync(userId, courseId, progressPercentage);
            return Ok(enrollment);
        }

        // Enroll in a learning path
        [HttpPost("learningpath/{pathId}/user/{userId}")]
        public async Task<IActionResult> EnrollInLearningPath(int pathId, int userId)
        {
            await _enrollmentService.EnrollInLearningPathAsync(userId, pathId);
            return Ok();
        }

        // Get an enrollment by ID
        [HttpGet("{enrollmentId}")]
        public async Task<IActionResult> GetEnrollment(int enrollmentId)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                return NotFound();
            }
            return Ok(enrollment);
        }

        // Get all enrolled courses for a user
        [HttpGet("user/{userId}/courses")]
        public async Task<IActionResult> GetAllEnrolledCourses(int userId)
        {
            var courses = await _enrollmentService.GetAllEnrolledCoursesAsync(userId);
            if (courses == null || !courses.Any())
            {
                return NotFound();  // No courses found for the user
            }
            return Ok(courses);  // Return the list of courses
        }

        //get all
        [HttpGet("total/enrollments/count")]
        public async Task<IActionResult> GetTotalEnrollmentsCount()
        {
            var totalEnrollments = await _enrollmentService.GetTotalEnrollmentsCountAsync();
            return Ok(totalEnrollments);  // Return the total count of enrollments
        }

        // delete
        [HttpDelete("course/{courseId}/user/{userId}")]
        public async Task<IActionResult> DeleteEnrollment(int userId, int courseId)
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(userId, courseId);
            if (result)
            {
                return NoContent();  // Successfully deleted
            }
            else
            {
                return NotFound();  // Enrollment not found
            }
        }
    }
}
