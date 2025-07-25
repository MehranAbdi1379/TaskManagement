using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Models;

public class AppTask : BaseEntity
{
    public AppTask()
    {
    }

    public AppTask(string title, string description, TaskStatus taskStatus, int ownerId,
        TaskPriority priority = TaskPriority.Medium)
    {
        SetTitle(title);
        SetDescription(description);
        SetStatus(taskStatus);
        SetPriority(priority);
        SetOwnerId(ownerId);
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateTime? DueDate { get; private set; }
    public int OwnerId { get; private set; }

    public ApplicationUser Owner { get; set; }
    public ICollection<TaskComment> Comments { get; set; }
    public ICollection<TaskAssignmentRequest> TaskAssignmentRequests { get; set; }

    public void SetDueDate(DateTime dueDate)
    {
        if (dueDate.Date < DateTime.Today)
            throw new DomainException("Due date cannot be in the past");
        DueDate = dueDate;
    }

    public void SetPriority(TaskPriority priority)
    {
        if (!Enum.IsDefined(typeof(TaskPriority), priority))
            throw new DomainException($"Invalid Task Priority Code with code {priority} for Task Id {Id}");
        Priority = priority;
    }

    public void SetTitle(string title)
    {
        ValidateTitle(title);
        Title = title;
    }

    private void ValidateTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) throw new DomainException("Title of the task can not be empty.");
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetStatus(TaskStatus taskTaskStatus)
    {
        ValidateStatus(taskTaskStatus);
        Status = taskTaskStatus;
    }

    private void ValidateStatus(TaskStatus taskTaskStatus)
    {
        if (!Enum.IsDefined(typeof(TaskStatus), taskTaskStatus))
            throw new DomainException($"Invalid Task Status Code with code {taskTaskStatus} for Task Id {Id}");
    }

    public void SetOwnerId(int userId)
    {
        OwnerId = userId;
    }
}