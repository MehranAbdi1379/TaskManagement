using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Models
{
    public class BaseNotification: BaseEntity
    {
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public NotificationType Type { get; private set; }
        public bool IsRead { get; private set; }

        public BaseNotification()
        {
            
        }

        public BaseNotification(int userId, string title, string content, NotificationType type)
        {
            SetUserId(userId);
            SetTitle(title);
            SetContent(content);
            SetType(type);
            IsRead = false;
        }

        public void SetUserId(int userId)
        {
            UserId = userId;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetContent(string content)
        {
            Content = content;
        }

        public void SetType(NotificationType notificationType)
        {
            Type = notificationType;
        }

        public void SetIsRead(bool isRead)
        {
            IsRead = isRead;
        }
    }
}
