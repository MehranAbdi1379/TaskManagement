using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Service.DTOs;
using TaskManagement.Shared.ServiceInterfaces;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.Repository.Repositories
{
    public class TaskCommentRepository : BaseRepository<TaskComment>, ITaskCommentRepository
    {
        private readonly IUserContext userContext;
        public TaskCommentRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
        {
            this.userContext = userContext;
        }

        public async Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsAsync(TaskCommentQueryParameters queryParams, int taskId)
        {
            var query = _context.TaskComments.AsQueryable();

            query = query.Where(taskComment => taskComment.Deleted == false);

            var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskId).Select(tu => tu.UserId).ToList();
            taskUsers.Append(_context.Tasks.Where(t => t.OwnerId == userContext.UserId).First().OwnerId);

            if (!taskUsers.Any(tu => tu == userContext.UserId)) throw new Exception("User does not have permission to access the task.");

            var users = _context.Users.Where(u => taskUsers.Any(tu => tu == u.Id));

            query = query.Where(task => task.TaskId == taskId);

            // Sorting by CreatedAt
            query = queryParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(task => task.CreatedAt)
                : query.OrderBy(task => task.CreatedAt);

            // Paging
            var totalCount = await query.CountAsync();
            var taskComments = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(taskComment => new TaskCommentResponseDto
                {
                    Id = taskComment.Id,
                    TaskId = taskComment.TaskId,
                    UserFullName = users
                    .Where(u => u.Id == taskComment.UserId)
                    .Select(u => u.FirstName + " " + u.LastName)
                    .First(),
                    CreatedAt = taskComment.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<TaskCommentResponseDto>
            {
                Items = taskComments,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }

        public async Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsByTaskAndUserIdAsync(TaskCommentQueryParameters queryParams, int taskId)
        {
            var query = _context.TaskComments.AsQueryable();

            query = query.Where(taskComment => taskComment.Deleted == false);

            var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskId).Select(tu => tu.UserId).ToList();
            taskUsers.Append(_context.Tasks.Where(t => t.OwnerId == userContext.UserId).First().OwnerId);

            if (!taskUsers.Any(tu => tu == userContext.UserId)) throw new Exception("User does not have permission to access the task.");

            var user = _context.Users.First(u => u.Id == userContext.UserId);

            query = query.Where(task => task.TaskId == taskId);

            // Sorting by CreatedAt
            query = queryParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(task => task.CreatedAt)
                : query.OrderBy(task => task.CreatedAt);

            // Paging
            var totalCount = await query.CountAsync();
            var taskComments = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(taskComment => new TaskCommentResponseDto
                {
                    Id = taskComment.Id,
                    TaskId = taskComment.TaskId,
                    UserFullName = user.FirstName + " " + user.LastName,
                    CreatedAt = taskComment.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<TaskCommentResponseDto>
            {
                Items = taskComments,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }

        public async Task DeleteTaskCommentAsync(int id)
        {
            var taskComment = await _dbSet.FirstAsync(x => x.Id == id && x.Deleted == false);
            if (taskComment.UserId != userContext.UserId) throw new Exception("Task Comment does not belong to the user.");
            taskComment.Deleted = true;
            _dbSet.Update(taskComment);
            await _context.SaveChangesAsync();
        }
    }
}
