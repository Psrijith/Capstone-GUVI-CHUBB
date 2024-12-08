using demobkend.Data;
using demobkend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demobkend.Services
{
    public class FeedbackService
    {
        private readonly AppDbContext _context;

        public FeedbackService(AppDbContext context)
        {
            _context = context;
        }

        // Create new rating/feedback
        public async Task<CourseRating> CreateRatingAsync(CourseRating courseRating)
        {
            if (courseRating == null)
                throw new ArgumentNullException(nameof(courseRating));

            courseRating.RatingDate = DateTime.UtcNow;

            _context.CourseRatings.Add(courseRating);
            await _context.SaveChangesAsync();

            return courseRating;
        }

        // Get feedback/ratings by course ID
        public async Task<IEnumerable<CourseRating>> GetRatingsByCourseIdAsync(int courseId)
        {
            return await _context.CourseRatings
                .Where(cr => cr.CourseId == courseId)
                .ToListAsync();
        }

        // Get feedback/ratings by user ID
        public async Task<IEnumerable<CourseRating>> GetRatingsByUserIdAsync(int userId)
        {
            return await _context.CourseRatings
                .Where(cr => cr.UserId == userId)
                .ToListAsync();
        }

        // Get a specific rating by rating ID
        public async Task<CourseRating> GetRatingByIdAsync(int ratingId)
        {
            return await _context.CourseRatings
                .FirstOrDefaultAsync(cr => cr.RatingId == ratingId);
        }
    }
}
