namespace TaskManagement.Domain.Models;

public class TaskAssignmentNotification : BaseEntity
{
    public TaskAssignmentNotification()
    {
    }

    public TaskAssignmentNotification(int notificationId, bool accepted, int requestId)
    {
        SetAccepted(accepted);
        SetRequestId(requestId);
        SetNotificationId(notificationId);
    }

    public int NotificationId { get; private set; }
    public bool Accepted { get; private set; }
    public int RequestId { get; private set; }

    public BaseNotification Notification { get; set; }
    public TaskAssignmentRequest Request { get; set; }

    public void SetAccepted(bool accepted)
    {
        Accepted = accepted;
    }

    public void SetRequestId(int requestId)
    {
        RequestId = requestId;
    }

    public void SetNotificationId(int notificationId)
    {
        NotificationId = notificationId;
    }
}