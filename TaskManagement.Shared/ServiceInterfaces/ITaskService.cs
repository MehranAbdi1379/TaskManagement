

using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Service.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto);
        Task DeleteTaskByIdAsync(int id);
        Task<TaskResponseDto> GetTaskByIdAsync(int id);
        Task<TaskResponseDto> UpdateTaskAsync(UpdateTaskDto dto);
        Task<TaskResponseDto> UpdateTaskStatusAsync(UpdateTaskStatusDto dto);
        Task<PagedResult<TaskResponseDto>> GetAllTasksAsync(TaskQueryParameters parameters);
        Task<BaseNotification> RequestTaskAssignmentAsync(string assigneeEmail, int taskId);
        Task<BaseNotification> RespondToTaskAssignmentAsync(int requestId, bool accept);
    }
}