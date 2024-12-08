using demobkend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demobkend.Repositories
{
    public interface ICourseContentRepository
    {
        Task<CourseContent> AddCourseContentAsync(CourseContent courseContent);
        Task<List<CourseContent>> GetCourseContentsAsync(int courseId);
    }
}
