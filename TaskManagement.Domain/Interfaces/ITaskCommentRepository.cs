using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskCommentRepository : IBaseRepository<TaskComment>
{
    Task DeleteTaskCommentAsync(int id);

    Task<(List<TaskComment> comments, int totalCount)> GetTaskCommentsAsync(int pageNumber, int pageSize,
        string sortOrder,
        int taskId);

    Task<(List<TaskComment> comments, int totalCount)> GetTaskCommentsByTaskAndUserIdAsync(int pageSize, int pageNumber,
        string sortOrder,
        int taskId);

    Task<(TaskComment, List<BaseNotification>)> AddTaskCommentAsync(TaskComment taskComment,
        List<(int userId, int taskId)> taskUserGroups);
}