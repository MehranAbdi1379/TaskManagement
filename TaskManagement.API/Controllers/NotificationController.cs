using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Service.Services;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        protected readonly INotificationService notificationService;
        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetUserActiveNotifications([FromQuery] NotificationQueryParameters parameters)
        {
            return Ok(await notificationService.GetUserNotificationsAsync(parameters));
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetUserNotificationHistory([FromQuery] NotificationQueryParameters parameters)
        {
            return Ok(await notificationService.GetNotificationHistoryAsync(parameters));
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateNotificationIsRead([FromBody] int notificationId)
        {
            await notificationService.UpdateNotificationIsReadAsync(notificationId);
            return Ok("Notification is updated.");
        }
    }
}
