using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Filters;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskRepository : IBaseRepository<AppTask>
{
    Task<(List<AppTask> tasks, int totalCount)> GetTasksAsync(int pageNumber, int pageSize,
        TaskStatus? status, TaskPriority? priority, DateTime? dueDate,
        TaskSortOptions sortOption, bool desc);

    Task<AppTask> GetTaskByIdAsync(int id);
    Task DeleteTaskAsync(int id);
    Task<List<ApplicationUser>> GetTaskAssignedUsersAsync(int taskId);
    Task<int> GetTaskCountByStatusAsync(TaskStatus status, TaskReportFilters filters);
    Task<int> GetTaskCountByPriorityAsync(TaskPriority priority, TaskReportFilters filters);
    Task<int> GetTotalTaskCountAsync(TaskReportFilters filters);
    Task<int> GetOverDueCountAsync(TaskReportFilters filters);
    Task<int> GetDueTodayCountAsync(TaskReportFilters filters);
    Task<int> GetDeletedTaskCountAsync(TaskReportFilters filters);
}