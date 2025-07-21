using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Shared.DTOs.Task;

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsOwner { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}