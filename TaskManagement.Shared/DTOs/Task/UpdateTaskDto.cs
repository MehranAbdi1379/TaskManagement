using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Shared.DTOs.Task;

public class UpdateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
}