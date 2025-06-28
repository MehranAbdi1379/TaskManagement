using TaskManagement.Domain.Enums;

namespace TaskManagement.Shared.DTOs.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

    }
}
