using Microsoft.EntityFrameworkCore;
using demobkend.Models;
using demobkend.models;

namespace demobkend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<LearningPath> LearningPaths { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<Progress> Progress { get; set; }
        public DbSet<CourseRating> CourseRatings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<InstructorDashboard> InstructorDashboards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration for the relationship between Course and Instructor (User)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)  // Instructor is the navigation property
                .WithMany()  // An Instructor can have many courses
                .HasForeignKey(c => c.InstructorId)  // InstructorId is the foreign key in Course
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete (if needed)
             
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany()  // Assuming many enrollments can belong to one course
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany()  // Assuming a user can have many enrollments
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<LearningPath>()
                .HasMany(lp => lp.Courses)
                .WithMany(c => c.LearningPaths)   
                .UsingEntity(j => j.ToTable("LearningPathCourses"));

            modelBuilder.Entity<Progress>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Progress>()
                .HasOne(p => p.CourseContent)
                .WithMany()
                .HasForeignKey(p => p.ContentId);
            // Additional configurations can go here, if needed.
        }
    }


}
