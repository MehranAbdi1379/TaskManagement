using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces;

public interface INotificationRepository : IBaseRepository<BaseNotification>
{
    Task<(List<BaseNotification>, int totalCount)> GetNotificationsByUserId(int pageNumber,
        int pageSize,
        string sortOrder, bool history);
}