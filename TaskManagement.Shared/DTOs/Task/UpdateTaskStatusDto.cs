using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Shared.DTOs.Task;

public class UpdateTaskStatusDto
{
    public TaskStatus TaskStatus { get; set; }
}