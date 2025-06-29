using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskCommentRepository : IBaseRepository<TaskComment>
{
    Task DeleteTaskCommentAsync(int id);

    Task<(List<TaskComment> comments, int totalCount)> GetTaskCommentsAsync(
        int pageNumber, int pageSize, string sortOrder,
        int taskId, bool ownedComments);

    Task<(TaskComment, List<BaseNotification>)> AddTaskCommentAsync(TaskComment taskComment,
        List<(int userId, int taskId)> taskUserGroups);
}