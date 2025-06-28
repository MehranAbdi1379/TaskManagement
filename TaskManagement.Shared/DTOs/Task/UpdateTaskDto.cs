using TaskManagement.Domain.Enums;

namespace TaskManagement.Shared.DTOs.Task
{
    public class UpdateTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

    }
}
