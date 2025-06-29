using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Models;

public class AppTask : BaseEntity
{
    public AppTask()
    {
    }

    public AppTask(string title, string description, Status taskStatus)
    {
        SetTitle(title);
        SetDescription(description);
        SetStatus(taskStatus);
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; }
    public int OwnerId { get; private set; }

    public ApplicationUser Owner { get; set; }
    public ICollection<ApplicationUser> AssignedUsers { get; set; }
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

    public void SetStatus(Status taskStatus)
    {
        ValidateStatus(taskStatus);
        Status = taskStatus;
    }

    private void ValidateStatus(Status taskStatus)
    {
        if (!Enum.IsDefined(typeof(Status), taskStatus))
            throw new DomainException($"Invalid Task Status Code with code {taskStatus} for Task Id {Id}");
    }

    public void SetOwnerId(int userId)
    {
        OwnerId = userId;
    }
}