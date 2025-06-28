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
        var query = _context.Tasks.AsQueryable();

        // Filter by Status (if provided)
        if (status.HasValue) query = query.Where(task => task.Status == status.Value);

        query = query.Where(task => task.Deleted == false);

        var taskUsers = _context.TaskUsers.Where(tu => tu.UserId == userContext.UserId).ToList();

        query = query.Where(task =>
            task.OwnerId == userContext.UserId ||
            taskUsers.Select(tu => tu.TaskId)
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
        var result = await _context.Tasks.Where(task => task.Id == id && task.Deleted == false).FirstAsync();

        var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == id).ToList();

        if (!taskUsers.Any(tu => tu.UserId == userContext.UserId)
            && userContext.UserId != result.OwnerId) throw new Exception("Task does not belong to the user.");
        return result;
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (task == null) throw new Exception("Task does not exist.");
        if (task.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
        task.Delete();
        _dbSet.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ApplicationUser>> GetTaskAssignedUsersAsync(int taskId)
    {
        var task = await _context.Tasks.Where(task => task.Id == taskId && task.Deleted == false).FirstOrDefaultAsync();
        if (task == null) throw new Exception("Task does not exist.");
        if (task.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
        var taskUsers = _context.TaskUsers.Where(tu => tu.TaskId == taskId).ToList();
        return _context.Users.Where(u => taskUsers.Select(tu => tu.UserId).Contains(u.Id)).ToList();
    }

    public async Task UnassignTaskAsync(int taskId, int userId)
    {
        var task = await _context.Tasks.Where(task => task.Id == taskId && task.Deleted == false).FirstOrDefaultAsync();
        if (task == null) throw new Exception("Task does not exist.");
        var taskUser = await _context.TaskUsers.Where(tu => tu.TaskId == taskId && tu.UserId == userId)
            .FirstOrDefaultAsync();
        if (taskUser == null) throw new Exception("User is not assigned to task.");
        _context.TaskUsers.Remove(taskUser);
        await _context.SaveChangesAsync();
    }
}