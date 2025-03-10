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

namespace TaskManagement.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository baseRepository;

        public NotificationService(INotificationRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public async Task CreateNotification(int userId, string title, string content, NotificationType notificationType)
        {
            var notification = new BaseNotification(userId, title, content, notificationType);

            await baseRepository.AddAsync(notification);
        }

        public async Task<PagedResult<NotificationResponseDto>> GetUserNotificationsAsync(NotificationQueryParameters parameters)
        {
            return await baseRepository.GetNotificationsByUserId(parameters);
        }
    }
}
