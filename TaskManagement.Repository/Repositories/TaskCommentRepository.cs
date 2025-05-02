using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;
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
        private readonly INotificationRepository notificationRepository;
        public TaskCommentRepository(TaskManagementDbContext context, IUserContext userContext, INotificationRepository notificationRepository) : base(context)
        {
            this.userContext = userContext;
            this.notificationRepository = notificationRepository;
        }

        public async Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsAsync(TaskCommentQueryParameters queryParams, int taskId)
        {
            var query = _context.TaskComments.AsQueryable();

            query = query.Where(taskComment => taskComment.Deleted == false);

            var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskId).Select(tu => tu.UserId).ToList();

            var taskOwner = _context.Tasks.Where(t => t.Id == taskId).First();

            taskUsers = taskUsers.Append(taskOwner.OwnerId).ToList();
            
            var users = _context.Users.Where(u => taskUsers.Any(tu => tu == u.Id)).ToList();

            //users.Add(_context.Users.Where(u => _context.Tasks.Where(t => t.Id == taskId).First().OwnerId == u.Id).First());

            if (!taskUsers.Any(tu => tu == userContext.UserId)) throw new Exception("User does not have permission to access the task.");


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
                    CreatedAt = taskComment.CreatedAt,
                    Text = taskComment.Text,
                    UserId = taskComment.UserId,
                    IsOwner = taskComment.UserId == userContext.UserId
                })
                .ToListAsync();

            foreach (var taskComment in taskComments)
            {
                taskComment.UserFullName = users.Where(x => x.Id == taskComment.UserId).Select(x => x.FirstName + " " + x.LastName).First();
            }
            
            var commentNotifications = new List<CommentNotification>();

            foreach (var taskComment in taskComments)
            {
                var commentNotification = await _context.CommentNotifications.FirstOrDefaultAsync(x => x.CommentId == taskComment.Id);
                if(commentNotification != null && 
                   await _context.Notifications.AnyAsync(x => x.Id == commentNotification.NotificationId && x.IsRead == false)) commentNotifications.Add(commentNotification);
            }

            foreach (var commentNotification in commentNotifications)
            {
                _context.Notifications.Remove(await _context.Notifications.FirstAsync(x => x.Id == commentNotification.NotificationId));
                _context.CommentNotifications.Remove(await _context.CommentNotifications.FirstAsync(x => x.Id == commentNotification.Id));
                await _context.SaveChangesAsync();
            }

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

            var taskOwner = _context.Tasks.Where(t => t.Id == taskId).First();

            taskUsers = taskUsers.Append(taskOwner.OwnerId).ToList();

            if (!taskUsers.Any(tu => tu == userContext.UserId)) throw new Exception("User does not have permission to access the task.");

            var user = _context.Users.First(u => u.Id == userContext.UserId);

            query = query.Where(task => task.TaskId == taskId && task.UserId == userContext.UserId);

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
                    CreatedAt = taskComment.CreatedAt,
                    Text = taskComment.Text,
                    IsOwner = taskComment.UserId == userContext.UserId
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

        public async Task<(TaskComment,List<BaseNotification>)> AddTaskCommentAsync(TaskComment taskComment, List<(int userId,int taskId)> taskUserGroups)
        {
            var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskComment.TaskId).Select(tu => tu.UserId).ToList();

            var taskOwner = _context.Tasks.Where(t => t.Id == taskComment.TaskId).First();

            taskUsers = taskUsers.Append(taskOwner.OwnerId).ToList();

            if (!taskUsers.Any(tu => tu == userContext.UserId)) throw new Exception("User does not have permission to access the task.");

            await _dbSet.AddAsync(taskComment);
            await _context.SaveChangesAsync();

            foreach (var taskUserGroup in taskUserGroups)
            {
                var shouldRemove = false;
                var userToRemove = 0;
                foreach (var taskUserId in taskUsers)
                {
                    if(taskUserId == taskUserGroup.userId && taskUserGroup.taskId == taskComment.TaskId)
                    {
                        shouldRemove = true;
                        userToRemove = taskUserId;
                        break;
                    }
                }

                if (shouldRemove) taskUsers.Remove(userToRemove);
            }
            
            var baseNotifications = new List<BaseNotification>();
            foreach (var taskUser in taskUsers)
            {
                var user = await _context.Users.FirstAsync(u => u.Id == userContext.UserId);
                var baseNotification = await notificationRepository.AddAsync(new BaseNotification(
                    taskUser, "New Task Comment",  user.FirstName + " " + user.LastName + ": "  
                                                  + taskComment.Text, NotificationType.General));
                baseNotifications.Add(baseNotification);
            }
            
            return (taskComment, baseNotifications);
        }
    }
}
