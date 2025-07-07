using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskRepository : IBaseRepository<AppTask>
{
    Task<(List<AppTask> tasks, int totalCount)> GetTasksAsync(int pageNumber, int pageSize, Status? status,
        string sortOrder);

    Task<AppTask> GetTaskByIdAsync(int id);
    Task DeleteTaskAsync(int id);
    Task<List<ApplicationUser>> GetTaskAssignedUsersAsync(int taskId);

    Task UnassignTaskAsync(int taskId, int userId);
    Task AssignTaskAsync(int taskId, ApplicationUser user);
}