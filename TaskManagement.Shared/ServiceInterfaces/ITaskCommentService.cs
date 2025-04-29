using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.Service.Services
{
    public interface ITaskCommentService
    {
        Task<(TaskCommentResponseDto, List<BaseNotification>)> CreateTaskCommentAsync(CreateTaskCommentDto dto,
            int taskId, List<(int userId, int taskId)> taskUserGroups);
        Task<TaskComment> DeleteTaskCommentByIdAsync(int id);
        Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsByTaskIdAsync(TaskCommentQueryParameters parameters, int taskId);
    }
}