using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories;

public class NotificationRepository : BaseRepository<BaseNotification>, INotificationRepository
{
    private readonly IUserContext userContext;

    public NotificationRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
    {
        this.userContext = userContext;
    }

    public async Task<(List<BaseNotification>, int totalCount)> GetNotificationsByUserId(int pageNumber,
        int pageSize,
        string sortOrder, bool history)
    {
        var query = _context.Notifications.AsQueryable().AsNoTracking();

        query = query.Where(notification => notification.Deleted == false);

        query = query.Where(notification =>
            notification.IsRead == history && notification.UserId == userContext.UserId);

        // Sorting by CreatedAt
        query = sortOrder.ToLower() == "desc"
            ? query.OrderByDescending(task => task.CreatedAt)
            : query.OrderBy(task => task.CreatedAt);

        // Paging
        var totalCount = await query.CountAsync();
        var notifications = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (notifications, totalCount);
    }
}