using demobkend.Models;
using demobkend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace demobkend.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public NotificationService(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Create and save a notification for a user, and send an email notification
        public async Task CreateNotificationAsync(int userId, string type, string message)
        {
            try
            {
                // Create the notification
                var notification = new Notification
                {
                    UserId = userId,
                    Type = type,
                    Message = message,
                    Timestamp = DateTime.UtcNow
                };

                // Add the notification to the database
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                // Get the user email to send the notification email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user != null)
                {
                    // Construct the email message
                    var emailSubject = $"New Notification: {type}";
                    var emailBody = $"Hello {user.Username},\n\nYou have a new notification:\n\n{message}\n\nTimestamp: {notification.Timestamp}\n\nBest regards,\nYour Learning Platform";

                    // Send the email (asynchronously)
                    await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
                }
                else
                {
                    // Handle case if user is not found
                    Console.WriteLine("User not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the notification creation process
                Console.WriteLine($"Error creating notification: {ex.Message}");
            }
        }

        // Get all notifications for a user
        public async Task<List<Notification>> GetNotificationsAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle any errors during the retrieval of notifications
                Console.WriteLine($"Error retrieving notifications: {ex.Message}");
                return new List<Notification>();  // Return an empty list if error occurs
            }
        }
    }
}
