using TaskManagement.Domain.Enums;

namespace TaskManagement.Shared.DTOs.Task
{
    public class UpdateTaskStatusDto
    {
        public int Id { get; set; }
        public Status Status { get; set; }
    }
}
