using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Repository.Repositories;
using TaskManagement.Service.DTOs;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository baseRepository;
        private readonly IUserContext userContext;


        public NotificationService(INotificationRepository baseRepository, IUserContext userContext)
        {
            this.baseRepository = baseRepository;
            this.userContext = userContext;
        }

        public async Task<BaseNotification> CreateNotification(int userId, string title, string content, NotificationType notificationType)
        {
            var notification = new BaseNotification(userId, title, content, notificationType);

            return await baseRepository.AddAsync(notification);
        }

        public async Task<PagedResult<NotificationResponseDto>> GetUserNotificationsAsync(NotificationQueryParameters parameters)
        {
            return await baseRepository.GetActiveNotificationsByUserId(parameters);
        }

        public async Task<PagedResult<NotificationResponseDto>> GetNotificationHistoryAsync(NotificationQueryParameters parameters)
        {
            return await baseRepository.GetNotificationHistoryByUserId(parameters);
        }

        public async Task UpdateNotificationIsReadAsync(int notificationId)
        {
            var notification = await baseRepository.GetByIdAsync(notificationId);
            if (notification.UserId != userContext.UserId) throw new Exception("Notification does not belong to the user.");

            notification.SetIsRead(true);

            await baseRepository.UpdateAsync(notification);
        }
    }
}
