using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Models;

public class AppTask : BaseEntity
{
    public AppTask()
    {
    }

    public AppTask(string title, string description, TaskStatus taskTaskStatus)
    {
        SetTitle(title);
        SetDescription(description);
        SetStatus(taskTaskStatus);
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskStatus TaskStatus { get; private set; }
    public int OwnerId { get; private set; }

    public ApplicationUser Owner { get; set; }
    public ICollection<TaskComment> Comments { get; set; }
    public ICollection<TaskAssignmentRequest> TaskAssignmentRequests { get; set; }

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
        TaskStatus = taskTaskStatus;
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