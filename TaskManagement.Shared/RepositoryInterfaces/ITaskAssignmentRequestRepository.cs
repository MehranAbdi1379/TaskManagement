using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.Repositories
{
    public interface ITaskAssignmentRequestRepository: IBaseRepository<TaskAssignmentRequest>
    {
        Task<TaskAssignmentRequest> GetTaskAssignmentRequestByNotificationIdAsync(int notificationId);
        Task<bool> RequestAlreadyExists(int assigneeId, int taskId);
    }
}