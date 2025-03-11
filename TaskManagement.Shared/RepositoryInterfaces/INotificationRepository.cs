using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Shared.DTOs.Notification;

namespace TaskManagement.Repository.Repositories
{
    public interface INotificationRepository: IBaseRepository<BaseNotification>
    {
        Task<PagedResult<NotificationResponseDto>> GetActiveNotificationsByUserId(NotificationQueryParameters queryParams);
        Task<PagedResult<NotificationResponseDto>> GetNotificationHistoryByUserId(NotificationQueryParameters queryParams);
    }
}