using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories;

public class TaskRepository : BaseRepository<AppTask>, ITaskRepository
{
    private readonly IUserContext userContext;

    public TaskRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
    {
        this.userContext = userContext;
    }

    public async Task<(List<AppTask> tasks, int totalCount)> GetTasksAsync(int pageNumber, int pageSize, Status? status,
        string sortOrder)
    {
        var query = _context.Tasks.AsQueryable().AsNoTracking();

        // Filter by Status (if provided)
        if (status.HasValue) query = query.Where(task => task.Status == status.Value);

        query = query.Where(task => task.Deleted == false);

        var currentUser = await _context.Users.Select(u => new
            {
                User = u,
                AssignedTasks = u.AssignedTasks.Select(at => at.Id)
            })
            .FirstOrDefaultAsync(u => u.User.Id == userContext.UserId);
        if (currentUser == null) throw new Exception($"User not found with id {userContext.UserId}");

        var taskUsers = currentUser.AssignedTasks;

        query = query.Where(task =>
            task.OwnerId == userContext.UserId ||
            taskUsers
                .Contains(task.Id));

        // Sorting by UpdatedAt
        query = sortOrder.ToLower() == "desc"
            ? query.OrderByDescending(task => task.UpdatedAt)
            : query.OrderBy(task => task.UpdatedAt);

        // Paging
        var totalCount = await query.CountAsync();
        var tasks = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (tasks, totalCount);
    }

    public async Task<AppTask> GetTaskByIdAsync(int id)
    {
        var task = await _context.Tasks.AsNoTracking().Where(task => task.Id == id && task.Deleted == false)
            .Select(t => new
            {
                Task = t, t.AssignedUsers
            }).FirstOrDefaultAsync();
        if (task == null) throw new Exception($"Task not found with id {id}");

        if (task.AssignedUsers.All(tu => tu.Id != userContext.UserId) && userContext.UserId != task.Task.OwnerId)
            throw new Exception("Task does not belong to the user.");
        return task.Task;
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (task == null) throw new Exception($"Task does not exist with id {id}.");
        if (task.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
        await DeleteAsync(task);
    }

    public async Task<List<ApplicationUser>> GetTaskAssignedUsersAsync(int taskId)
    {
        var task = await _context.Tasks.Where(task => task.Id == taskId && task.Deleted == false)
            .Include(appTask => appTask.AssignedUsers).FirstOrDefaultAsync();
        if (task == null) throw new Exception($"Task does not exist with id {taskId}.");
        if (task.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
        return task.AssignedUsers.ToList();
    }

    public async Task UnassignTaskAsync(int taskId, int userId)
    {
        var task = await _context.Tasks.Where(task => task.Id == taskId && task.Deleted == false)
            .Include(appTask => appTask.AssignedUsers).FirstOrDefaultAsync();
        if (task == null) throw new Exception("Task does not exist.");
        var taskUser = task.AssignedUsers.FirstOrDefault(u => u.Id == userId);
        if (taskUser == null) throw new Exception("User is not assigned to task.");
        task.AssignedUsers.Remove(taskUser);
        await _context.SaveChangesAsync();
    }
}
