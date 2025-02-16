using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Models
{
    public class AppTask: BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Status { get; private set; }

        public AppTask(string title, string description, string status)
        {
            SetTitle(title);
            SetDescription(description);
            SetStatus(status);
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

        public void SetStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) throw new DomainException("Status of the task can not be empty.");
            Status = status;
        }
    }
}
