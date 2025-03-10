using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories
{
    public class NotificationRepository : BaseRepository<BaseNotification>, INotificationRepository
    {
        protected readonly IUserContext userContext;
        public NotificationRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
        {
            this.userContext = userContext;
        }

        public async Task<PagedResult<NotificationResponseDto>> GetNotificationsByUserId(NotificationQueryParameters queryParams)
        {
            var query = _context.Notifications.AsQueryable();

            query = query.Where(notification => notification.Deleted == false && notification.UserId == userContext.UserId);

            // Sorting by CreatedAt
            query = queryParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(task => task.CreatedAt)
                : query.OrderBy(task => task.CreatedAt);

            // Paging
            var totalCount = await query.CountAsync();
            var tasks = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(notification => new NotificationResponseDto
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Content = notification.Content,
                    IsRead = notification.IsRead,
                    Type = notification.Type.ToString(),
                    UserId = notification.UserId
                })
                .ToListAsync();

            return new PagedResult<NotificationResponseDto>
            {
                Items = tasks,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }
    }
}
