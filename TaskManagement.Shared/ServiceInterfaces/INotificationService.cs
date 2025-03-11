using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Shared.DTOs.Notification;

namespace TaskManagement.Service.Services
{
    public interface INotificationService
    {
        Task<BaseNotification> CreateNotification(int userId, string title, string content, NotificationType notificationType);
        Task<PagedResult<NotificationResponseDto>> GetUserNotificationsAsync(NotificationQueryParameters parameters);
        Task<PagedResult<NotificationResponseDto>> GetNotificationHistoryAsync(NotificationQueryParameters parameters);
        Task UpdateNotificationIsReadAsync(int notificationId);
    }
}