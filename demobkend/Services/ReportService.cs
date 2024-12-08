using demobkend.Data;
using demobkend.Models;
using Microsoft.EntityFrameworkCore;

namespace demobkend.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        // Generate and save a report (Course Enrollment or User Activity)
        public async Task<Report> GenerateReportAsync(string type, int createdBy, string data)
        {
            var report = new Report
            {
                Type = type,
                GeneratedDate = DateTime.UtcNow,
                Data = data,
                CreatedBy = createdBy
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return report;
        }

        // Get all reports (you can add filters such as type, user, etc.)
        public async Task<List<Report>> GetReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        // Get a report by its ID
        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports.FirstOrDefaultAsync(r => r.ReportId == reportId);
        }
    }
}
