using demobkend.Data;
using demobkend.Models;
using Microsoft.EntityFrameworkCore;

namespace demobkend.Services
{
    public class QuizService
    {
        private readonly AppDbContext _context;

        public QuizService(AppDbContext context)
        {
            _context = context;
        }

        // Create a new quiz
        public async Task<Quiz> CreateQuizAsync(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        // Get quizzes for a specific course
        public async Task<List<Quiz>> GetQuizzesByCourseIdAsync(int courseId)
        {
            return await _context.Quizzes
                .Where(q => q.CourseId == courseId)
                .ToListAsync();
        }

        // Get a quiz by ID
        public async Task<Quiz> GetQuizByIdAsync(int quizId)
        {
            return await _context.Quizzes
                .FirstOrDefaultAsync(q => q.QuizId == quizId);
        }
    }

}
