using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task.Enums;

namespace TaskManagement.Repository.Extensions.Task;

public class TaskSorterByDueDate : ITaskSorter
{
    public TaskSortOptions SortOption => TaskSortOptions.DueDate;

    public IQueryable<AppTask> Sort(IQueryable<AppTask> query, bool desc)
    {
        return desc ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority);
    }
}