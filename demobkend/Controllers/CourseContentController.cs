using demobkend.Models;
using demobkend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using demobkend.Repositories;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseContentController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseContentController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // POST: api/coursecontent/{courseId}
        [HttpPost("{courseId}")]
        public async Task<IActionResult> AddCourseContent(int courseId, [FromBody] CourseContent courseContent)
        {
            if (courseContent == null)
            {
                return BadRequest("Course content data is required.");
            }

            try
            {
                var createdContent = await _courseService.AddCourseContentAsync(courseId, courseContent);
                return CreatedAtAction(nameof(GetCourseContents), new { courseId = courseId }, createdContent);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding content: {ex.Message}");
            }
        }

        // GET: api/coursecontent/{courseId}
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseContents(int courseId)
        {
            try
            {
                var contents = await _courseService.GetCourseContentsAsync(courseId);
                return Ok(contents);
            }
            catch (Exception ex)
            {
                return NotFound($"Error fetching content: {ex.Message}");
            }
        }
    }
}
