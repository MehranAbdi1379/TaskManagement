using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.API.Controllers;

[Route("api/notifications")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService notificationService;

    public NotificationController(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserActiveNotifications([FromQuery] NotificationQueryParameters parameters)
    {
        return Ok(await notificationService.GetUserNotificationsAsync(parameters, parameters.History));
    }

    [HttpPatch("{notificationId:int}")]
    public async Task<IActionResult> UpdateNotificationIsRead(int notificationId)
    {
        await notificationService.UpdateNotificationIsReadAsync(notificationId);
        return Ok("Notification is updated.");
    }
}