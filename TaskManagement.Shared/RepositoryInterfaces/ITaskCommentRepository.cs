using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.Repository.Repositories
{
    public interface ITaskCommentRepository: IBaseRepository<TaskComment>
    {
        Task DeleteTaskCommentAsync(int id);
        Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsByTaskAndUserIdAsync(TaskCommentQueryParameters queryParams, int taskId);
        Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsAsync(TaskCommentQueryParameters queryParams, int taskId);
        Task<(TaskComment, List<BaseNotification>)> AddTaskCommentAsync(TaskComment taskComment,
            List<(int userId, int taskId)> taskUserGroups);
    }
}