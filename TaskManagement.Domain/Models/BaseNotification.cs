using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Models;

public class BaseNotification : BaseEntity
{
    public BaseNotification()
    {
    }

    public BaseNotification(int userId, string title, string content, NotificationType type)
    {
        SetUserId(userId);
        SetTitle(title);
        SetContent(content);
        SetType(type);
    }

    public int UserId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public NotificationType Type { get; private set; }
    public bool IsRead { get; private set; }

    public void SetUserId(int userId)
    {
        UserId = userId;
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be null or empty.");
        Title = title;
    }

    public void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Content cannot be null or empty.");
        Content = content;
    }

    public void SetType(NotificationType notificationType)
    {
        if (!Enum.IsDefined(typeof(NotificationType), notificationType))
            throw new DomainException("Invalid notification type.");
        Type = notificationType;
    }

    public void ReadNotification()
    {
        IsRead = true;
    }
}