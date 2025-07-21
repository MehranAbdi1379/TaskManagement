using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Shared.DTOs.Task;

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsOwner { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
}