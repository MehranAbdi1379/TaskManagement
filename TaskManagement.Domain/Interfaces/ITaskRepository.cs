using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskRepository : IBaseRepository<AppTask>
{
    Task<(List<AppTask> tasks, int totalCount)> GetTasksAsync(int pageNumber, int pageSize, TaskStatus? status,
        string sortOrder);

    Task<AppTask> GetTaskByIdAsync(int id);
    Task DeleteTaskAsync(int id);
    Task<List<ApplicationUser>> GetTaskAssignedUsersAsync(int taskId);
}