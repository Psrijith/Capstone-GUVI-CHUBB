using demobkend.DTOs;
using demobkend.models;
using demobkend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demobkend.Repositories
{
    public interface ICourseService
    {
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<List<CourseDTO>> GetAllCoursesAsync();
        Task<Course> CreateCourseAsync(CreateCourseRequest request, int userId);
        Task<Course> UpdateCourseAsync(int courseId, UpdateCourseRequest request, int userId);
        Task<bool> DeleteCourseAsync(int courseId, int userId);

        Task<CourseContent> AddCourseContentAsync(int courseId, CourseContent courseContent);
        Task<List<CourseContent>> GetCourseContentsAsync(int courseId);
         

    }
}
