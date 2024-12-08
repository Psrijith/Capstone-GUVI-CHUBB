using demobkend.DTOs;
using demobkend.Models;
using demobkend.Repositories;
using demobkend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using demobkend.Data;
using Microsoft.EntityFrameworkCore;
using demobkend.models;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly AppDbContext _context;  // Add this to access the DB context

        public CourseController(ICourseService courseService, AppDbContext context)
        {
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // POST: api/course
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")] 
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (request == null)
            {
                return BadRequest("Course data is required.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid user ID.");
            }
            
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null)
            {
                return Unauthorized("Role is not assigned to the user.");
            }

            if (roleClaim.Value == "Instructor" || roleClaim.Value == "Admin")
            {
                var instructor = await _context.Users
                    .FirstOrDefaultAsync(u => u.Role == "Instructor");

                if (instructor == null || !instructor.IsActive)
                {
                    return Unauthorized("Instructor account is not active.");
                }
            }

            try
            {
                var course = await _courseService.CreateCourseAsync(request, userId);
                return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while creating the course: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<ActionResult<Course>> GetAllCourses()
        {
            var course = await _courseService.GetAllCoursesAsync();
            if (course == null)
                return NotFound($"No Course Available , Notify Admin/Instructor");

            return Ok(course);
        }


        // GET: api/course/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found.");

            return Ok(course);
        }

        // PUT: api/course/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
        {
            if (request == null)
            {
                return BadRequest("Course data is required.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid user ID.");
            }

            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null)
            {
                return Unauthorized("Role is not assigned to the user.");
            }

            if (roleClaim.Value == "Instructor")
            {
                var instructor = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.Role == "Instructor");

                if (instructor == null || !instructor.IsActive)
                {
                    return Unauthorized("Instructor account is not active.");
                }
            }

            try
            {
                // Fetch the course, including the Instructor (whole entity)
                var course = await _context.Courses
                    .Include(c => c.Instructor)  // Include the full Instructor entity
                    .FirstOrDefaultAsync(c => c.CourseId == id);

                if (course == null)
                {
                    return NotFound($"Course with ID {id} not found.");
                }

                // Update the course data based on the request
                course.Title = request.Title ?? course.Title;
                course.Description = request.Description ?? course.Description;
                course.Duration = !string.IsNullOrEmpty(request.Duration) && TimeSpan.TryParse(request.Duration, out TimeSpan parsedDuration)? parsedDuration: course.Duration;
                // Update other fields as necessary from the request

                // Optionally, if you need to check if the user is the instructor of this course
                if (roleClaim.Value == "Instructor" && course.Instructor.UserId != userId)
                {
                    return Unauthorized("You are not the instructor of this course.");
                }

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return NoContent(); // Successfully updated
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the course: {ex.Message}");
            }
        }


       


        // DELETE: api/course/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var userClaim = User.FindFirst("sub");
            if (userClaim == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            if (!int.TryParse(userClaim.Value, out int userId))
            {
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                var success = await _courseService.DeleteCourseAsync(id, userId);
                if (!success)
                {
                    return NotFound($"Course with ID {id} not found.");
                }

                return NoContent(); // Successfully deleted
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the course: {ex.Message}");
            }
        }
    }
}
