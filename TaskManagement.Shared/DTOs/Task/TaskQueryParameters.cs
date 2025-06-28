using TaskManagement.Domain.Enums;

namespace TaskManagement.Shared.DTOs.Task
{
    public class TaskQueryParameters
    {
        public int PageNumber { get; set; } = 1;  // Default to first page
        public int PageSize { get; set; } = 10;   // Default page size
        public Status? Status { get; set; }       // Optional filter
        public string SortOrder { get; set; } = "asc"; // "asc" or "desc"
    }
}
