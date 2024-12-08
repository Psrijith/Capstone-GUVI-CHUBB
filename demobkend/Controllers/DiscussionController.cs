using demobkend.Models;
using demobkend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly DiscussionService _discussionService;

        public DiscussionController(DiscussionService discussionService)
        {
            _discussionService = discussionService;
        }

        // POST: api/Assessment/discussion
        [HttpPost("discussion")]
        public async Task<IActionResult> CreateDiscussion([FromBody] Discussion discussion)
        {
            if (discussion == null)
            {
                return BadRequest("Discussion content is null.");
            }

            // Create the discussion
            var createdDiscussion = await _discussionService.CreateDiscussionAsync(discussion);

            return CreatedAtAction(nameof(GetDiscussionById), new { id = createdDiscussion.DiscussionId }, createdDiscussion);
        }

        // GET: api/Assessment/discussion/5
        [HttpGet("discussion/{id}")]
        public async Task<ActionResult<Discussion>> GetDiscussionById(int id)
        {
            var discussion = await _discussionService.GetDiscussionByIdAsync(id);

            if (discussion == null)
            {
                return NotFound();
            }

            return Ok(discussion);
        }

        // GET: api/Assessment/discussions
        [HttpGet("discussions")]
        public async Task<ActionResult<IEnumerable<Discussion>>> GetAllDiscussions()
        {
            var discussions = await _discussionService.GetAllDiscussionsAsync();

            return Ok(discussions);
        }

        // PUT: api/Assessment/discussion/5/close
        [HttpPut("discussion/{id}/close")]
        public async Task<IActionResult> CloseDiscussion(int id)
        {
            var result = await _discussionService.CloseDiscussionAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Assessment/discussion/5/open
        [HttpPut("discussion/{id}/open")]
        public async Task<IActionResult> OpenDiscussion(int id)
        {
            var result = await _discussionService.OpenDiscussionAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
