using TaskManagement.Domain.Enums;

namespace TaskManagement.Shared.DTOs.Task;

public class UpdateTaskStatusDto
{
    public Status Status { get; set; }
}