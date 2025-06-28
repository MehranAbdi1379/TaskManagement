using TaskManagement.Domain.Enums;

namespace TaskManagement.Shared.DTOs.Task
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsOwner { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
