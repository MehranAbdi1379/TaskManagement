using TaskManagement.Service.DTOs;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.Service.Services
{
    public interface ITaskCommentService
    {
        Task<TaskCommentResponseDto> CreateTaskCommentAsync(CreateTaskCommentDto dto, int taskId);
        Task DeleteTaskCommentByIdAsync(int id);
        Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsByTaskIdAsync(TaskCommentQueryParameters parameters, int taskId);
    }
}