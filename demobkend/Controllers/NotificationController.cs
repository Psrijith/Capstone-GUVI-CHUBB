using demobkend.Models;
using demobkend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Get notifications for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(userId);
            return Ok(notifications);
        }
    }
}
