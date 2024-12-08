using demobkend.Data;
using demobkend.Models;
using demobkend.Repositories;
using demobkend.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using demobkend.models;
using System.Collections.Generic;

namespace demobkend.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly AppDbContext _context;

        public CourseService(ICourseRepository courseRepository, AppDbContext context)
        {
            _courseRepository = courseRepository;
            _context = context;
        }

        // Implement GetCourseByIdAsync
        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _courseRepository.GetCourseByIdAsync(courseId);
        }

        // Corrected GetAllCoursesAsync to return List<CourseDTO>
        public async Task<List<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Select(c => new CourseDTO
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    InstructorId = c.Instructor.UserId,
                    Instructor = new InstructorDTO
                    {
                        Username = c.Instructor.Username,
                        Email = c.Instructor.Email
                    },
                    Category = c.Category,
                    DifficultyLevel = c.DifficultyLevel,
                    Duration = c.Duration.ToString(),
                    Status = c.Status
                })
                .ToListAsync();

            return courses;
        }

        // Implement CreateCourseAsync
        public async Task<Course> CreateCourseAsync(CreateCourseRequest request, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            // Remove the check for instructor status. Now, anyone can create a course.

            if (TimeSpan.TryParse(request.Duration, out var duration) && duration.TotalMinutes <= 0)
            {
                throw new ArgumentException("Duration must be a positive value.");
            }

            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                DifficultyLevel = request.DifficultyLevel,
                Duration = duration,
                Status = request.Status,
                InstructorId = userId // Assigning the user who created the course as the instructor
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }





        // Implement UpdateCourseAsync
        public async Task<Course> UpdateCourseAsync(int courseId, UpdateCourseRequest request, int userId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);

            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            if (course.InstructorId != userId && !await _context.Users.AnyAsync(u => u.UserId == userId && u.Role == "Admin"))
                throw new UnauthorizedAccessException("You do not have permission to update this course.");

            var instructor = await _context.Users.FindAsync(course.InstructorId);
            if (instructor == null || !instructor.IsActive)
                throw new UnauthorizedAccessException("The instructor must be approved before making changes.");

            course.Title = request.Title;
            course.Description = request.Description;
            course.Category = request.Category;
            course.DifficultyLevel = request.DifficultyLevel;
            course.Duration = TimeSpan.Parse(request.Duration);
            course.Status = request.Status;

            return await _courseRepository.UpdateCourseAsync(course);
        }


        // Implement DeleteCourseAsync
        public async Task<bool> DeleteCourseAsync(int courseId, int userId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);

            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            if (course.InstructorId != userId && !await _context.Users.AnyAsync(u => u.UserId == userId && u.Role == "Admin"))
                throw new UnauthorizedAccessException("You do not have permission to delete this course.");

            return await _courseRepository.DeleteCourseAsync(courseId);
        }



        // course content sake
        public async Task<CourseContent> AddCourseContentAsync(int courseId, CourseContent courseContent)
        {
            // Ensure the course exists
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            // Set the courseId for the content
            courseContent.CourseId = courseId;

            // Add the course content to the context and save
            _context.CourseContents.Add(courseContent);
            await _context.SaveChangesAsync();

            return courseContent;
        }

        // Get Course Contents
        public async Task<List<CourseContent>> GetCourseContentsAsync(int courseId)
        {
            var contents = await _context.CourseContents
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.ContentOrder) // Ensure content is ordered
                .ToListAsync();

            return contents;
        }



        

    }
}
