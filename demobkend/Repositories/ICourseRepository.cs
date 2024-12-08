using demobkend.DTOs;
using demobkend.models;

namespace demobkend.Repositories
{
    public interface ICourseRepository
    {
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<List<CourseDTO>> GetAllCoursesAsync();
        Task<Course> CreateCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int courseId);
    }
}
