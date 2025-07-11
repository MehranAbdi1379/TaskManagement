using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Shared.ServiceInterfaces;

public interface ITaskService
{
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto);
    Task DeleteTaskByIdAsync(int id);
    Task<TaskResponseDto> GetTaskByIdAsync(int id);
    Task<TaskResponseDto> UpdateTaskAsync(UpdateTaskDto dto, int taskId);
    Task<TaskResponseDto> UpdateTaskStatusAsync(UpdateTaskStatusDto dto, int taskId);
    Task<PagedResult<TaskResponseDto>> GetAllTasksAsync(TaskQueryParameters parameters);
    Task<BaseNotification> RequestTaskAssignmentAsync(string assigneeEmail, int taskId);
    Task<BaseNotification> RespondToTaskAssignmentAsync(int requestId, bool accept);
    Task<List<TaskAssignedUserResponseDto>> GetTaskAssignedUsers(int taskId);
    Task<BaseNotification> UnassignTaskAsync(int taskId, int userId);
}