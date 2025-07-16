using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskAssignmentRequestRepository : IBaseRepository<TaskAssignmentRequest>
{
    Task<TaskAssignmentRequest> GetTaskAssignmentRequestByNotificationIdAsync(int notificationId);
    Task<bool> RequestAlreadyExists(int assigneeId, int taskId);
    Task<TaskAssignmentRequest> GetTaskAssignmentRequestByUserIdAndTaskIdAsync(int userId, int taskId);
}