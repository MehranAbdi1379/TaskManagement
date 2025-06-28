using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces;

public interface INotificationRepository : IBaseRepository<BaseNotification>
{
    Task<(List<BaseNotification>, int totalCount)> GetActiveNotificationsByUserId(int pageNumber,
        int pageSize,
        string sortOrder);

    Task<(List<BaseNotification> notifications, int totalCount)> GetNotificationHistoryByUserId(
        int pageSize, int pageNumber,
        string sortOrder);
}