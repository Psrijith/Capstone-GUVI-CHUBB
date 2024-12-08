using demobkend.Data;
using demobkend.DTOs;
using demobkend.models;
using demobkend.Models;
using Microsoft.EntityFrameworkCore;

namespace demobkend.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Instructor.Username)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }
         

        public async Task<List<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)  // Include the entire Instructor entity
                .Select(c => new CourseDTO
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    InstructorId = c.Instructor.UserId,
                    Instructor = new InstructorDTO
                    {
                        Username = c.Instructor.Username,
                        Email = c.Instructor.Email
                    },
                    Category = c.Category,
                    DifficultyLevel = c.DifficultyLevel,
                    Duration = c.Duration.ToString(),
                    Status = c.Status
                })
                .ToListAsync();

            return courses;
        }






        public async Task<Course> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
         
    }
}
