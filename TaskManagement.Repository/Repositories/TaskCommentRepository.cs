using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories;

public class TaskCommentRepository : BaseRepository<TaskComment>, ITaskCommentRepository
{
    private readonly INotificationRepository notificationRepository;
    private readonly IUserContext userContext;

    public TaskCommentRepository(TaskManagementDBContext context, IUserContext userContext,
        INotificationRepository notificationRepository) : base(context)
    {
        this.userContext = userContext;
        this.notificationRepository = notificationRepository;
    }

    public async Task DeleteTaskCommentAsync(int id)
    {
        var taskComment = await _dbSet.FirstAsync(x => x.Id == id && x.Deleted == false);
        if (taskComment.UserId != userContext.UserId) throw new Exception("Task Comment does not belong to the user.");
        taskComment.Delete();
        _dbSet.Update(taskComment);
        await _context.SaveChangesAsync();
    }

    public async Task<(TaskComment, List<BaseNotification>)> AddTaskCommentAsync(TaskComment taskComment,
        List<(int userId, int taskId)> taskUserGroups)
    {
        var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskComment.TaskId).Select(tu => tu.UserId)
            .ToList();

        var taskOwner = _context.Tasks.First(t => t.Id == taskComment.TaskId);

        taskUsers = taskUsers.Append(taskOwner.OwnerId).ToList();

        if (taskUsers.All(tu => tu != userContext.UserId))
            throw new Exception("User does not have permission to access the task.");

        await _dbSet.AddAsync(taskComment);
        await _context.SaveChangesAsync();

        foreach (var taskUserGroup in taskUserGroups)
        {
            var shouldRemove = false;
            var userToRemove = 0;
            foreach (var taskUserId in taskUsers)
                if (taskUserId == taskUserGroup.userId && taskUserGroup.taskId == taskComment.TaskId)
                {
                    shouldRemove = true;
                    userToRemove = taskUserId;
                    break;
                }

            if (shouldRemove) taskUsers.Remove(userToRemove);
        }

        var baseNotifications = new List<BaseNotification>();
        foreach (var taskUser in taskUsers)
        {
            var user = await _context.Users.FirstAsync(u => u.Id == userContext.UserId);
            var baseNotification = await notificationRepository.AddAsync(new BaseNotification(
                taskUser, "New Task Comment", user.FirstName + " " + user.LastName + ": "
                                              + taskComment.Text, NotificationType.General));
            baseNotifications.Add(baseNotification);
        }

        return (taskComment, baseNotifications);
    }

    public async Task<(List<TaskComment> comments, int totalCount)> GetTaskCommentsAsync(
        int pageNumber, int pageSize, string sortOrder,
        int taskId)
    {
        var query = _context.TaskComments.AsQueryable().AsNoTracking();

        //TODO: Add soft delete to global ef core options so you don't have to filter out these data each time
        query = query.Where(taskComment => taskComment.Deleted == false);

        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) throw new Exception("Task does not exist.");

        var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskId).Select(tu => tu.UserId).ToList();

        taskUsers = taskUsers.Append(task.OwnerId).ToList();

        var users = _context.Users.Where(u => taskUsers.Any(tu => tu == u.Id)).ToList();

        if (taskUsers.All(tu => tu != userContext.UserId))
            throw new Exception("User does not have permission to access the task.");


        query = query.Where(taskComment => taskComment.TaskId == taskId);

        // Sorting by CreatedAt
        query = sortOrder.ToLower() == "desc"
            ? query.OrderByDescending(task => task.CreatedAt)
            : query.OrderBy(task => task.CreatedAt);

        // Paging
        var totalCount = await query.CountAsync();
        var taskComments = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var commentNotifications = new List<CommentNotification>();

        foreach (var taskComment in taskComments)
        {
            var commentNotification =
                await _context.CommentNotifications.FirstOrDefaultAsync(x => x.CommentId == taskComment.Id);
            if (commentNotification != null &&
                await _context.Notifications.AnyAsync(x =>
                    x.Id == commentNotification.NotificationId && x.IsRead == false))
                commentNotifications.Add(commentNotification);
        }

        foreach (var commentNotification in commentNotifications)
        {
            _context.Notifications.Remove(
                await _context.Notifications.FirstAsync(x => x.Id == commentNotification.NotificationId));
            _context.CommentNotifications.Remove(
                await _context.CommentNotifications.FirstAsync(x => x.Id == commentNotification.Id));
            await _context.SaveChangesAsync();
        }

        return (taskComments, totalCount);
    }

    public async Task<(List<TaskComment> comments, int totalCount)> GetTaskCommentsByTaskAndUserIdAsync(
        int pageSize, int pageNumber, string sortOrder, int taskId)
    {
        var query = _context.TaskComments.AsQueryable();

        query = query.Where(taskComment => taskComment.Deleted == false);

        var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskId).Select(tu => tu.UserId).ToList();

        var taskOwner = _context.Tasks.Where(t => t.Id == taskId).First();

        taskUsers = taskUsers.Append(taskOwner.OwnerId).ToList();

        if (!taskUsers.Any(tu => tu == userContext.UserId))
            throw new Exception("User does not have permission to access the task.");

        var user = _context.Users.First(u => u.Id == userContext.UserId);

        query = query.Where(task => task.TaskId == taskId && task.UserId == userContext.UserId);

        // Sorting by CreatedAt
        query = sortOrder.ToLower() == "desc"
            ? query.OrderByDescending(task => task.CreatedAt)
            : query.OrderBy(task => task.CreatedAt);

        // Paging
        var totalCount = await query.CountAsync();
        var taskComments = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (taskComments, totalCount);
    }
}