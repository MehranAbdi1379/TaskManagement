using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models
{
    public class TaskAssignmentRequest: BaseEntity
    {
        public int TaskOwnerId { get; private set; }
        public int AssigneeId { get; private set; }
        public int TaskId { get; private set; }
        public bool IsAccepted { get; private set; } = false;
        public int RequestNotificationId { get; private set; }

        public TaskAssignmentRequest()
        {
            
        }

        public TaskAssignmentRequest(int taskOwnerId, int assigneeId, int taskId)
        {
            SetTaskOwnerId(taskOwnerId);
            SetAssigneeId(assigneeId);
            SetTaskId(taskId);
        }

        public void SetRequestNotificationId(int requestNotificationId)
        {
            RequestNotificationId = requestNotificationId;
        }

        public void SetTaskOwnerId(int taskOwnerId)
        {
            TaskOwnerId = taskOwnerId;
        }

        public void SetAssigneeId(int assigneeId)
        {
            AssigneeId = assigneeId;
        }

        public void SetTaskId(int taskId)
        {
            TaskId = taskId;
        }

        public void SetIsAccepted(bool isAccepted)
        {
            IsAccepted = isAccepted;
        }
    }
}
