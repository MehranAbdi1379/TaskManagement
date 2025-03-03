using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories
{
    public class TaskRepository : BaseRepository<AppTask>, ITaskRepository
    {
        private readonly IUserContext userContext;
        public TaskRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
        {
            this.userContext = userContext;
        }

        public async Task<PagedResult<TaskResponseDto>> GetTasksAsync(TaskQueryParameters queryParams)
        {
            var query = _context.Tasks.AsQueryable();

            // Filter by Status (if provided)
            if (queryParams.Status.HasValue)
            {
                query = query.Where(task => task.Status == queryParams.Status.Value);
            }

            query = query.Where(task => task.Deleted == false);

            query = query.Where(task => task.OwnerId == userContext.UserId);

            // Sorting by CreatedAt
            query = queryParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(task => task.UpdatedAt)
                : query.OrderBy(task => task.UpdatedAt);

            // Paging
            var totalCount = await query.CountAsync();
            var tasks = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(task => new TaskResponseDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    CreatedAt = task.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<TaskResponseDto>
            {
                Items = tasks,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }

        public async Task<AppTask> GetTaskByIdAsync(int id)
        {
            var result = await _context.Tasks.Where(task => task.Id == id && task.Deleted == false).FirstAsync();

            if (result.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
            return result;
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _dbSet.FirstAsync(x => x.Id == id && x.Deleted == false);
            if (task.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
            task.Deleted = true;
            _dbSet.Update(task);
            await _context.SaveChangesAsync();
        }

    }
}
