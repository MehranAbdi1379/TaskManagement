using TaskManagement.Domain.Enums;
using TaskManagement.Shared.DTOs.Task.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Shared.DTOs.Task;

public class TaskQueryParameters
{
    public int PageNumber { get; set; } = 1; // Default to first page
    public int PageSize { get; set; } = 10; // Default page size
    public TaskStatus? Status { get; set; } // Optional filter
    public TaskPriority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskSortOptions SortOptions { get; set; } = TaskSortOptions.UpdatedDate;
    public bool Desc { get; set; } = true;
}