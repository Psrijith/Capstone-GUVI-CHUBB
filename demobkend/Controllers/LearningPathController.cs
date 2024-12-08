using demobkend.Data;
using demobkend.Models;
using demobkend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningPathController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LearningPathController(AppDbContext context)
        {
            _context = context;
        }

        // Create a new Learning Path (using DTO)
        [HttpPost]
        public async Task<IActionResult> CreateLearningPath([FromBody] LearningPathDTO learningPathDTO)
        {
            if (learningPathDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            // Validate if all course IDs exist in the database
            var courses = await _context.Courses
                .Where(c => learningPathDTO.CourseIds.Contains(c.CourseId))
                .ToListAsync();

            if (courses.Count != learningPathDTO.CourseIds.Count)
            {
                return BadRequest("Some course IDs are invalid.");
            }

            // Create the LearningPath object based on DTO
            var learningPath = new LearningPath
            {
                Name = learningPathDTO.Name,
                Description = learningPathDTO.Description,
                CompletionCriteria = learningPathDTO.CompletionCriteria,
                CertificationAwarded = learningPathDTO.CertificationAwarded,
                Courses = courses // Add the valid courses to the LearningPath
            };

            // Add the new learning path to the database
            _context.LearningPaths.Add(learningPath);
            await _context.SaveChangesAsync();

            // Return the created LearningPath object
            return CreatedAtAction(nameof(GetLearningPathById), new { id = learningPath.PathId }, learningPath);
        }

        // Get all Learning Paths
        [HttpGet]
        public async Task<ActionResult<List<LearningPath>>> GetLearningPaths()
        {
            var learningPaths = await _context.LearningPaths
                .Include(lp => lp.Courses)  // Include related courses
                .ToListAsync();

            return Ok(learningPaths);
        }

        // Get a Learning Path by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<LearningPath>> GetLearningPathById(int id)
        {
            var learningPath = await _context.LearningPaths
                .Include(lp => lp.Courses)  // Include related courses
                .FirstOrDefaultAsync(lp => lp.PathId == id);

            if (learningPath == null)
            {
                return NotFound();
            }

            return Ok(learningPath);
        }

        // Update Learning Path (optional)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLearningPath(int id, [FromBody] LearningPathDTO learningPathDTO)
        {
            var learningPath = await _context.LearningPaths
                .Include(lp => lp.Courses)
                .FirstOrDefaultAsync(lp => lp.PathId == id);

            if (learningPath == null)
            {
                return NotFound();
            }

            // Validate if all course IDs exist in the database
            var courses = await _context.Courses
                .Where(c => learningPathDTO.CourseIds.Contains(c.CourseId))
                .ToListAsync();

            if (courses.Count != learningPathDTO.CourseIds.Count)
            {
                return BadRequest("Some course IDs are invalid.");
            }

            // Update the learning path
            learningPath.Name = learningPathDTO.Name;
            learningPath.Description = learningPathDTO.Description;
            learningPath.CompletionCriteria = learningPathDTO.CompletionCriteria;
            learningPath.CertificationAwarded = learningPathDTO.CertificationAwarded;
            learningPath.Courses = courses;  // Update the courses

            _context.LearningPaths.Update(learningPath);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a Learning Path
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLearningPath(int id)
        {
            var learningPath = await _context.LearningPaths.FindAsync(id);

            if (learningPath == null)
            {
                return NotFound();
            }

            _context.LearningPaths.Remove(learningPath);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // DTOs (Data Transfer Objects) used for creating LearningPath and Course objects
    public class LearningPathDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> CourseIds { get; set; }  // List of course IDs
        public string CompletionCriteria { get; set; }
        public bool CertificationAwarded { get; set; }
    }

    public class CourseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int InstructorId { get; set; }
        public string Category { get; set; }
        public string DifficultyLevel { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }
        public List<int> LearningPathIds { get; set; }  // List of LearningPath IDs
    }
}
