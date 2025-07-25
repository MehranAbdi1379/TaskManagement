using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Filters;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Repository.Extensions.Task;
using TaskManagement.Shared.DTOs.Task.Enums;
using TaskManagement.Shared.ServiceInterfaces;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Repository.Repositories;

public class TaskRepository : BaseRepository<AppTask>, ITaskRepository
{
    private readonly IUserContext userContext;

    public TaskRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
    {
        this.userContext = userContext;
    }

    public async Task<AppTask> GetTaskByIdAsync(int id)
    {
        var task = await _context.Tasks.AsNoTracking().Where(task => task.Id == id && task.Deleted == false)
            .Include(t => t.TaskAssignmentRequests.Where(tar => tar.IsAccepted && tar.Deleted == false))
            .FirstOrDefaultAsync();
        if (task == null) throw new Exception($"Task not found with id {id}");

        if (task.TaskAssignmentRequests.All(tu => tu.AssigneeId != userContext.UserId) &&
            userContext.UserId != task.OwnerId)
            throw new Exception("Task does not belong to the user.");
        return task;
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
            .Include(appTask => appTask.TaskAssignmentRequests.Where(tar => tar.IsAccepted && tar.Deleted == false))
            .FirstOrDefaultAsync();
        if (task == null) throw new Exception($"Task does not exist with id {taskId}.");
        if (task.OwnerId != userContext.UserId) throw new Exception("Task does not belong to the user.");
        var assigneeIds = task.TaskAssignmentRequests.Select(tar => tar.AssigneeId);
        var users = _context.Users.Where(u => assigneeIds.Contains(u.Id));
        return users.ToList();
    }

    public async Task<(List<AppTask> tasks, int totalCount)> GetTasksAsync(int pageNumber, int pageSize,
        TaskStatus? status, TaskPriority? priority, DateTime? dueDate,
        TaskSortOptions sortOption, bool desc)
    {
        var query = _context.Tasks.AsQueryable().AsNoTracking();

        // Filter by Status (if provided)
        if (status.HasValue) query = query.Where(task => task.Status == status.Value);
        if (priority.HasValue) query = query.Where(task => task.Priority == priority.Value);
        if (dueDate.HasValue)
            query = query.Where(task => task.DueDate != null && task.DueDate.Value.Date == dueDate.Value.Date);

        query = query.Where(task => task.Deleted == false);

        var currentUser = await _context.Users.Select(u => new
            {
                User = u,
                AssignedTasks = u.AssignedTaskRequests.Where(tar => tar.IsAccepted && tar.Deleted == false)
                    .Select(at => at.TaskId)
            })
            .FirstOrDefaultAsync(u => u.User.Id == userContext.UserId);
        if (currentUser == null) throw new Exception($"User not found with id {userContext.UserId}");

        var taskUsers = currentUser.AssignedTasks;

        query = query.Where(task =>
            task.OwnerId == userContext.UserId ||
            taskUsers
                .Contains(task.Id));

        query = query.SortTasks(sortOption, desc);

        // Paging
        var totalCount = await query.CountAsync();
        var tasks = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (tasks, totalCount);
    }

    public async Task<int> GetTaskCountByStatusAsync(TaskStatus status, TaskReportFilters filters)
    {
        var query = GetTasksWithAssignmentRequestsForCurrentUser();
        query = FilterTaskByDates(query, filters);
        return await query.CountAsync(t =>
            t.Status == status);
    }

    public async Task<int> GetTaskCountByPriorityAsync(TaskPriority priority, TaskReportFilters filters)
    {
        var query = GetTasksWithAssignmentRequestsForCurrentUser();
        query = FilterTaskByDates(query, filters);
        return await query.CountAsync(t => t.Priority == priority);
    }

    public async Task<int> GetTotalTaskCountAsync(TaskReportFilters filters)
    {
        var query = GetTasksWithAssignmentRequestsForCurrentUser();
        query = FilterTaskByDates(query, filters);
        return await query.CountAsync();
    }

    public async Task<int> GetOverDueCountAsync(TaskReportFilters filters)
    {
        var query = GetTasksWithAssignmentRequestsForCurrentUser();
        query = FilterTaskByDates(query, filters);
        return await query.Where(t =>
                t.DueDate != null && t.DueDate.Value.Date <= DateTime.Now.Date &&
                t.Status != TaskStatus.Completed && t.Status != TaskStatus.Cancelled)
            .CountAsync();
    }

    public async Task<int> GetDueTodayCountAsync(TaskReportFilters filters)
    {
        var query = GetTasksWithAssignmentRequestsForCurrentUser();
        query = FilterTaskByDates(query, filters);
        return await query.CountAsync(t =>
            t.DueDate != null && t.DueDate.Value.Date == DateTime.Now.Date &&
            t.Status != TaskStatus.Completed && t.Status != TaskStatus.Cancelled);
    }

    public async Task<int> GetDeletedTaskCountAsync(TaskReportFilters filters)
    {
        var query = _context.Tasks.Include(t => t.TaskAssignmentRequests).AsNoTracking();
        query = query.Where(t => t.Deleted == true &&
                                 (t.TaskAssignmentRequests.Any(tar =>
                                      tar.IsAccepted && tar.Deleted == false && tar.AssigneeId == userContext.UserId) ||
                                  t.OwnerId == userContext.UserId));
        query = FilterTaskByDates(query, filters);
        return await query.CountAsync();
    }

    private IQueryable<AppTask> FilterTaskByDates(IQueryable<AppTask> query, TaskReportFilters filters)
    {
        if (filters.CreatedStartDate != null) query = query.Where(t => t.CreatedAt >= filters.CreatedStartDate);
        if (filters.CreatedEndDate != null) query = query.Where(t => t.CreatedAt <= filters.CreatedEndDate);
        if (filters.DueDateStartDate != null) query = query.Where(t => t.DueDate >= filters.DueDateStartDate);
        if (filters.DueDateEndDate != null) query = query.Where(t => t.DueDate <= filters.DueDateEndDate);
        if (filters.UpdatedStartDate != null) query = query.Where(t => t.UpdatedAt >= filters.UpdatedStartDate);
        if (filters.UpdatedEndDate != null) query = query.Where(t => t.UpdatedAt <= filters.UpdatedEndDate);
        return query;
    }

    private IQueryable<AppTask> GetTasksWithAssignmentRequestsForCurrentUser()
    {
        var query = _context.Tasks.Include(t => t.TaskAssignmentRequests).AsNoTracking();
        return query.Where(t => t.Deleted == false &&
                                (t.TaskAssignmentRequests.Any(tar =>
                                     tar.IsAccepted && tar.Deleted == false && tar.AssigneeId == userContext.UserId) ||
                                 t.OwnerId == userContext.UserId));
    }
}