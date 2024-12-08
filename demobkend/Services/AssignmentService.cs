using demobkend.Models;
using demobkend.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demobkend.Services
{
    public class AssignmentService
    {
        private readonly AppDbContext _context;

        public AssignmentService(AppDbContext context)
        {
            _context = context;
        }

        // Create a new assignment
        public async Task<Assignment> CreateAssignmentAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            // Notify enrolled users via email (or in-app notifications) here (optional)

            return assignment;
        }

        // Get an assignment by its ID
        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        {
            return await _context.Assignments
                                 .Where(a => a.AssignmentId == id)
                                 .FirstOrDefaultAsync();
        }

        // Get all assignments by course ID
        public async Task<IEnumerable<Assignment>> GetAssignmentsByCourseIdAsync(int courseId)
        {
            return await _context.Assignments
                                 .Where(a => a.CourseId == courseId)
                                 .ToListAsync();
        }

        // Update an existing assignment
        public async Task<Assignment> UpdateAssignmentAsync(Assignment updatedAssignment)
        {
            var existingAssignment = await _context.Assignments
                                                    .FirstOrDefaultAsync(a => a.AssignmentId == updatedAssignment.AssignmentId);

            if (existingAssignment == null)
            {
                return null; // Assignment not found
            }

            existingAssignment.Title = updatedAssignment.Title;
            existingAssignment.Description = updatedAssignment.Description;
            existingAssignment.DueDate = updatedAssignment.DueDate;
            existingAssignment.GradingCriteria = updatedAssignment.GradingCriteria;

            await _context.SaveChangesAsync();

            return existingAssignment;
        }

        // Delete an assignment by its ID
        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            var assignment = await _context.Assignments
                                           .FirstOrDefaultAsync(a => a.AssignmentId == id);

            if (assignment == null)
            {
                return false; // Assignment not found
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
