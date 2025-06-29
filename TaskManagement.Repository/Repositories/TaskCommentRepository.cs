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
        var taskComment = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (taskComment == null) throw new InvalidOperationException($"TaskComment with id {id} not found");
        if (taskComment.UserId != userContext.UserId) throw new Exception("Task Comment does not belong to the user.");
        await DeleteAsync(taskComment);
    }

    public async Task<(TaskComment, List<BaseNotification>)> AddTaskCommentAsync(TaskComment taskComment,
        List<(int userId, int taskId)> taskUserGroups)
    {
        var task = await _context.Tasks.Include(appTask => appTask.AssignedUsers)
            .FirstOrDefaultAsync(x => x.Id == taskComment.TaskId);
        if (task == null) throw new Exception($"Task with id {taskComment.TaskId} not found");

        var taskUsers = task.AssignedUsers.Select(u => u.Id).ToList();
        taskUsers = taskUsers.Append(task.OwnerId).ToList();

        if (taskUsers.All(tu => tu != userContext.UserId))
            throw new Exception("User does not have permission to access the task.");

        await AddAsync(taskComment);

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

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userContext.UserId);
        if (user == null) throw new InvalidOperationException($"User with id {userContext.UserId} not found");
        var baseNotifications = new List<BaseNotification>();
        foreach (var taskUser in taskUsers)
        {
            var baseNotification = await notificationRepository.AddAsync(new BaseNotification(
                taskUser, "New Task Comment", user.FirstName + " " + user.LastName + ": "
                                              + taskComment.Text, NotificationType.General));
            baseNotifications.Add(baseNotification);
        }

        return (taskComment, baseNotifications);
    }

    public async Task<(List<TaskComment> comments, int totalCount)> GetTaskCommentsAsync(
        int pageNumber, int pageSize, string sortOrder,
        int taskId, bool ownedComments)
    {
        var query = _context.TaskComments.AsQueryable().AsNoTracking();

        //TODO: Add soft delete to global ef core options so you don't have to filter out these data each time
        query = query.Where(taskComment => taskComment.Deleted == false);

        var task = _context.Tasks.Include(appTask => appTask.AssignedUsers).FirstOrDefault(t => t.Id == taskId);
        if (task == null) throw new Exception($"Task does not exist with id {taskId}.");

        var taskUserIds = task.AssignedUsers.Select(u => u.Id).ToList();

        taskUserIds = taskUserIds.Append(task.OwnerId).ToList();

        if (taskUserIds.All(tu => tu != userContext.UserId))
            throw new Exception("User does not have permission to access the task.");


        query = query.Where(taskComment => taskComment.TaskId == taskId);

        if (ownedComments) query = query.Where(taskComment => taskComment.UserId == userContext.UserId);

        // Sorting by CreatedAt
        query = sortOrder.ToLower() == "desc"
            ? query.OrderByDescending(t => t.CreatedAt)
            : query.OrderBy(t => t.CreatedAt);

        // Paging
        var totalCount = await query.CountAsync();
        var taskComments = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!ownedComments)
            foreach (var taskComment in taskComments)
            {
                var notification =
                    taskComment.Notifications.FirstOrDefault(n =>
                        n.UserId == userContext.UserId && n is { IsRead: false, Deleted: false });
                if (notification != null)
                    await notificationRepository.DeleteAsync(notification);
            }


        return (taskComments, totalCount);
    }
}