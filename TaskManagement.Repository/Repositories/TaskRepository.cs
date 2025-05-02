using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Shared.DTOs.Task;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories
{
    public class TaskRepository : BaseRepository<AppTask>, ITaskRepository
    {
        private readonly IUserContext userContext;
        public TaskRepository(TaskManagementDbContext context, IUserContext userContext) : base(context)
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

            var taskUsers = _context.TaskUsers.Where(tu => tu.UserId == userContext.UserId).ToList();

            query = query.Where(task =>
            task.OwnerId == userContext.UserId ||
            taskUsers.Select(tu => tu.TaskId)
            .Contains(task.Id));

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
                    IsOwner = task.OwnerId == userContext.UserId,
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

            var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == id).ToList();

            if (!taskUsers.Any(tu => tu.UserId == userContext.UserId) && userContext.UserId != result.OwnerId) throw new Exception("Task does not belong to the user.");
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

        public async Task<List<TaskAssignedUserResponseDto>> GetTaskAssignedUsersAsync(int taskId)
        {
            var task = await _context.Tasks.Where(task => task.Id == taskId && task.Deleted == false).FirstOrDefaultAsync();
            if(task == null) throw new Exception("Task does not not exist.");
            if(task.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
            var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskId ).ToList();
            return taskUsers.Select(tu => new TaskAssignedUserResponseDto
            {
                FirstName = _context.Users.First(u => u.Id == tu.UserId).FirstName,
                LastName = _context.Users.First(u => u.Id == tu.UserId).LastName,
                Email = _context.Users.First(u => u.Id == tu.UserId).Email,
                UserId = tu.UserId,
            }).ToList();
        }

        public async Task UnassignTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks.Where(task => task.Id == taskId && task.Deleted == false).FirstOrDefaultAsync();
            if(task == null) throw new Exception("Task does not exist.");
            var taskUser = await _context.TaskUsers.Where(tu => tu.TaskId == taskId && tu.UserId == userId).FirstOrDefaultAsync();
            if(taskUser == null) throw new Exception("User is not assigned to task.");
            _context.TaskUsers.Remove(taskUser);
            await _context.SaveChangesAsync();
        }
    }
}
