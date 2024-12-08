using demobkend.Data;
using demobkend.Models;
using Microsoft.EntityFrameworkCore;

namespace demobkend.Services
{
    public class AnalyticsService
    {
        private readonly AppDbContext _context;
        private readonly ReportService _reportService;

        public AnalyticsService(AppDbContext context, ReportService reportService)
        {
            _context = context;
            _reportService = reportService;
        }

        // Example: Generate a report about course enrollment statistics
        public async Task GenerateCourseEnrollmentReportAsync(int courseId, int createdBy)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {
                throw new Exception("Course not found");
            }

            // Fetch data for the report
            var totalEnrolled = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .CountAsync();

            var totalCompleted = await _context.Enrollments
                .Where(e => e.CourseId == courseId && e.CompletionStatus == "Completed")
                .CountAsync();

            var completionRate = (double)totalCompleted / totalEnrolled * 100;

            var reportData = $"Course: {course.Title}\n" +
                             $"Total Enrolled: {totalEnrolled}\n" +
                             $"Total Completed: {totalCompleted}\n" +
                             $"Completion Rate: {completionRate}%";

            // Generate and save the report
            await _reportService.GenerateReportAsync("Course Enrollment", createdBy, reportData);
        }

        // Example: Generate a report about user activity
        public async Task GenerateUserActivityReportAsync(int userId, int createdBy)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Fetch data for the report (example: user enrolled courses)
            var enrolledCourses = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.Course)  // Ensure you're eager loading the associated courses
                .ToListAsync();

            var reportData = $"User: {user.Username}\n" +
                             $"Enrolled Courses: {string.Join(", ", enrolledCourses.Select(e => e.Course.Title))}";

            // Generate and save the report
            await _reportService.GenerateReportAsync("User Activity", createdBy, reportData);
        }

        // Method to get all reports
        public async Task<List<Report>> GetReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        // Method to get a report by its ID
        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports.FirstOrDefaultAsync(r => r.ReportId == reportId);
        }

        public async Task<string> GenerateUserCourseReportAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                throw new Exception("User not found");

            // Fetch the enrolled courses with progress
            var enrollments = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.Course) // Ensure courses are loaded
                .ToListAsync();

            if (enrollments.Count == 0)
                return $"User {user.Username} has not enrolled in any courses.";

            // Build the report data
            var reportData = $"Report for User: {user.Username}\n\n";
            foreach (var enrollment in enrollments)
            {
                reportData += $"- Course: {enrollment.Course.Title}\n" +
                              $"  Progress: {enrollment.ProgressPercentage}%\n" +
                              $"  Status: {enrollment.CompletionStatus}\n" +
                              $"  Enrolled On: {enrollment.EnrollmentDate}\n\n";
            }

            return reportData;
        }

    }
}
