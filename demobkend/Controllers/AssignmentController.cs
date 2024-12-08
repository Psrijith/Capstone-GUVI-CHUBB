using demobkend.Models;
using demobkend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentService _assignmentService;

        public AssignmentController(AssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        // POST: api/Assignment
        [HttpPost]
        public async Task<IActionResult> CreateAssignment([FromBody] Assignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest("Assignment content is null.");
            }

            // Create assignment and notify users
            var createdAssignment = await _assignmentService.CreateAssignmentAsync(assignment);

            // Return the created assignment with a 201 Created status
            return CreatedAtAction(nameof(GetAssignmentById), new { id = createdAssignment.AssignmentId }, createdAssignment);
        }

        // GET: api/Assignment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Assignment>> GetAssignmentById(int id)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        // GET: api/Assignment/course/1
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignmentsByCourseId(int courseId)
        {
            var assignments = await _assignmentService.GetAssignmentsByCourseIdAsync(courseId);

            if (assignments == null || !assignments.Any())
            {
                return NotFound();
            }

            return Ok(assignments);
        }

        // PUT: api/Assignment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] Assignment assignment)
        {
            if (id != assignment.AssignmentId)
            {
                return BadRequest("Assignment ID mismatch.");
            }

            var updatedAssignment = await _assignmentService.UpdateAssignmentAsync(assignment);

            if (updatedAssignment == null)
            {
                return NotFound();
            }

            return NoContent(); // Return status code 204 for successful update
        }

        // DELETE: api/Assignment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var result = await _assignmentService.DeleteAssignmentAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent(); // Return status code 204 for successful deletion
        }
    }
}
