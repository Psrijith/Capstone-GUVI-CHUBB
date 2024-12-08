using demobkend.Services;
using Microsoft.AspNetCore.Mvc;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AnalyticsService _analyticsService;
        private readonly ReportService _reportService;

        public ReportController(AnalyticsService analyticsService, ReportService reportService)
        {
            _analyticsService = analyticsService;
            _reportService = reportService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserCourseReport(int userId)
        {
            try
            {
                // Generate the report
                var reportData = await _analyticsService.GenerateUserCourseReportAsync(userId);

                // Return the report as a response
                return Ok(new { report = reportData });
            }
            catch (Exception ex)
            {
                // Handle any errors
                return BadRequest(new { message = ex.Message });
            }
        }


        public class CreateReportRequest
        {
            public int CreatedBy { get; set; }
        }

        [HttpPost("course/{courseId}")]
        public async Task<IActionResult> GenerateCourseReport(int courseId, [FromBody] CreateReportRequest request)
        {
            try
            {
                await _analyticsService.GenerateCourseEnrollmentReportAsync(courseId, request.CreatedBy);
                return Ok("Course report generated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // POST: api/report/user/{userId}
        // This method generates a user activity report
        [HttpPost("user/{userId}")]
        public async Task<IActionResult> GenerateUserReport(int userId, [FromBody] int createdBy)
        {
            try
            {
                // Call the method to generate the report
                await _analyticsService.GenerateUserActivityReportAsync(userId, createdBy);

                // Return success response
                return Ok("User activity report generated successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
