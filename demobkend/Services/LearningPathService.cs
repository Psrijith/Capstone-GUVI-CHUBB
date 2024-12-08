using demobkend.Data;
using demobkend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace demobkend.Services
{
    public class LearningPathService
    {
        private readonly AppDbContext _context;

        public LearningPathService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);   
        }

        public async Task<LearningPath> CreateLearningPathAsync(LearningPath path)
        {
            _context.LearningPaths.Add(path);
            await _context.SaveChangesAsync();
            return path;
        }

        public async Task<List<LearningPath>> GetAllLearningPathsAsync()
        {
            return await _context.LearningPaths.ToListAsync();
        }

        public async Task<LearningPath> GetLearningPathByIdAsync(int pathId)
        {
            return await _context.LearningPaths
                .Include(lp => lp.Courses)  // Include related courses
                .FirstOrDefaultAsync(lp => lp.PathId == pathId);
        }
    }
}
