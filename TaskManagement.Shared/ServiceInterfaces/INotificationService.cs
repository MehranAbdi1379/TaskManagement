﻿using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs;
using TaskManagement.Shared.DTOs.Notification;

namespace TaskManagement.Shared.ServiceInterfaces;

public interface INotificationService
{
    Task<BaseNotification> CreateNotification(int userId, string title, string content,
        NotificationType notificationType);

    Task<PagedResult<NotificationResponseDto>> GetUserNotificationsAsync(NotificationQueryParameters parameters,
        bool history);

    Task UpdateNotificationIsReadAsync(int notificationId);
}