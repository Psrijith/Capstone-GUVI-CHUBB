using demobkend.Models;
using demobkend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using demobkend.models;

namespace demobkend.Services
{
    public class EnrollmentService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public EnrollmentService(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Enroll a learner in a course
        public async Task<Enrollment> EnrollInCourseAsync(int userId, int courseId)
        {
            // Check if the user is already enrolled in the course
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                // If the user is already enrolled, return null or throw an exception with a message
                throw new Exception("You are already enrolled in this course.");
            }

            // If not already enrolled, proceed with the enrollment
            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow,
                CompletionStatus = "InProgress",
                ProgressPercentage = 0
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

                if (course != null)
                {
                    var message = $"Hello {user.Username},\n\nYou have been successfully enrolled in the course: {course.Title}.";
                    await _emailService.SendEmailAsync(user.Email, "Course Enrollment Confirmation", message);
                }
            }

            return enrollment;
        }

        // Update progress for a course
        public async Task<Enrollment> UpdateProgressAsync(int userId, int courseId, double progressPercentage)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (enrollment == null)
                throw new Exception("Enrollment not found");

            enrollment.ProgressPercentage = progressPercentage;
            if (progressPercentage == 100)
                enrollment.CompletionStatus = "Completed";

            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
                if (course != null)
                {
                    var message = $"Hello {user.Username},\n\nYour progress in the course '{course.Title}' is now {progressPercentage}%.";
                    await _emailService.SendEmailAsync(user.Email, "Course Progress Update", message);
                }
            }

            return enrollment;
        }

        // Enroll in a learning path
        public async Task EnrollInLearningPathAsync(int userId, int pathId)
        {
            var learningPath = await _context.LearningPaths
                .Include(lp => lp.Courses)  
                .FirstOrDefaultAsync(lp => lp.PathId == pathId);

            if (learningPath == null)
            {
                throw new Exception("Learning Path not found");
            }

            foreach (var course in learningPath.Courses)
            {
                var enrollment = new Enrollment
                {
                    UserId = userId,
                    CourseId = course.CourseId,  
                    EnrollmentDate = DateTime.UtcNow,
                    CompletionStatus = "InProgress",
                    ProgressPercentage = 0
                };

                _context.Enrollments.Add(enrollment);
            }

            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                var message = $"Hello {user.Username},\n\nYou have been successfully enrolled in the learning path: {learningPath.Name}.";
                await _emailService.SendEmailAsync(user.Email, "Learning Path Enrollment", message);
            }
        }

        // Get enrollment by ID
        public async Task<Enrollment> GetEnrollmentByIdAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
            
            if (enrollment == null)
            {
                throw new Exception("Enrollment not found");
            }

            return enrollment;
        }

        //get all for user id
        public async Task<List<Course>> GetAllEnrolledCoursesAsync(int userId)
        {
            var enrollments = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.Course)  // Include related course data
                .ToListAsync();

            return enrollments.Select(e => e.Course).ToList();  // Return the list of courses
        }

        //get all 
        public async Task<int> GetTotalEnrollmentsCountAsync()
        {
            return await _context.Enrollments.CountAsync();   
        }

        //delete
        public async Task<bool> DeleteEnrollmentAsync(int userId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (enrollment == null)
            {
                // Enrollment not found
                return false;
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return true;  // Successfully deleted
        }
    }
}
