using demobkend.Models;
using demobkend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] CourseRating courseRating)
        {
            if (courseRating == null)
            {
                return BadRequest("Rating content is null.");
            }

            // Create rating
            var createdRating = await _feedbackService.CreateRatingAsync(courseRating);

            return CreatedAtAction(nameof(GetRatingById), new { id = createdRating.RatingId }, createdRating);
        }

        // GET: api/Feedback/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseRating>> GetRatingById(int id)
        {
            var rating = await _feedbackService.GetRatingByIdAsync(id);

            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }

        // GET: api/Feedback/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<CourseRating>>> GetRatingsByCourseId(int courseId)
        {
            var ratings = await _feedbackService.GetRatingsByCourseIdAsync(courseId);

            return Ok(ratings);
        }

        // GET: api/Feedback/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CourseRating>>> GetRatingsByUserId(int userId)
        {
            var ratings = await _feedbackService.GetRatingsByUserIdAsync(userId);

            return Ok(ratings);
        }
    }
}
