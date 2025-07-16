namespace TaskManagement.Domain.Models;

public class TaskAssignmentRequest : BaseEntity
{
    public TaskAssignmentRequest()
    {
    }

    public TaskAssignmentRequest(int assigneeId, int taskId)
    {
        SetAssigneeId(assigneeId);
        SetTaskId(taskId);
    }

    public int AssigneeId { get; private set; }
    public int TaskId { get; private set; }
    public bool IsAccepted { get; private set; }
    public int RequestNotificationId { get; private set; }

    public ApplicationUser Assignee { get; set; }
    public AppTask Task { get; set; }
    public BaseNotification RequestNotification { get; set; }

    public void SetRequestNotificationId(int requestNotificationId)
    {
        RequestNotificationId = requestNotificationId;
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