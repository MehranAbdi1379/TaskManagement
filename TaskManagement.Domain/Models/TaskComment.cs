using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Models
{
    public class TaskComment: BaseEntity
    {
        public int TaskId { get; private set; }
        public int UserId { get; private set; }
        public string Text { get; private set; }

        public TaskComment()
        {
            
        }

        public TaskComment(int taskId, int userId, string text)
        {
            SetTaskId(taskId);
            SetUserId(userId);
            SetText(text);
        }

        public void SetTaskId(int taskId)
        {
            TaskId = taskId;
        }

        public void SetUserId(int userId)
        {
            UserId = userId;
        }

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new DomainException("TaskComment text can not be empty.");
            Text = text;
        }
    }
}
