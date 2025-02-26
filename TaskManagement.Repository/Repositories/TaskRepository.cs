using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;

namespace TaskManagement.Repository.Repositories
{
    public class TaskRepository : BaseRepository<AppTask>, ITaskRepository
    {
        public TaskRepository(TaskManagementDBContext context) : base(context)
        {
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
    }
}
