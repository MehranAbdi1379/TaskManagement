namespace TaskManagement.Domain.Models;

public class CommentNotification: BaseEntity
{
    public int NotificationId { get; private set; }
    public int CommentId { get; private set; }

    public CommentNotification()
    {
            
    }

    public CommentNotification(int notificationId, int commentId)
    {
        SetNotificationId(notificationId);
        SetCommentId(commentId);
    }

    public void SetNotificationId(int userId)
    {
        NotificationId = userId;
    }

    public void SetCommentId(int taskId)
    {   
        CommentId = taskId;
    }
}