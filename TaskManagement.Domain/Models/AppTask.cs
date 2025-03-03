using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Models
{
    public class AppTask: BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Status Status { get; private set; }
        public int OwnerId { get; private set; }

        public AppTask()
        {

        }

        public AppTask(string title, string description, Status taskStatus)
        {
            SetTitle(title);
            SetDescription(description);
            SetStatus(taskStatus);
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) throw new DomainException("Title of the task can not be empty.");
            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetStatus(Status taskStatus)
        {
            if ((int)taskStatus < 0 || (int)taskStatus > 3) throw new DomainException("Task Status Code should be between 0 and 3");
            Status = taskStatus;
        }

        public void SetOwnerId(int userId)
        {
            OwnerId = userId;
        }
    }
}
