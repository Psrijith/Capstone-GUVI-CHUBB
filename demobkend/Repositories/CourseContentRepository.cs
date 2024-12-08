using demobkend.Data;
using demobkend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demobkend.Repositories
{
    public class CourseContentRepository : ICourseContentRepository
    {
        private readonly AppDbContext _context;

        public CourseContentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CourseContent> AddCourseContentAsync(CourseContent courseContent)
        {
            _context.CourseContents.Add(courseContent);
            await _context.SaveChangesAsync();
            return courseContent;
        }

        public async Task<List<CourseContent>> GetCourseContentsAsync(int courseId)
        {
            return await _context.CourseContents
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.ContentOrder) // Ensure content is ordered
                .ToListAsync();
        }
    }
}
