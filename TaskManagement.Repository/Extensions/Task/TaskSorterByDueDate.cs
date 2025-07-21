using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task.Enums;

namespace TaskManagement.Repository.Extensions.Task;

public class TaskSorterByDueDate : ITaskSorter
{
    public TaskSortOptions SortOption => TaskSortOptions.DueDate;

    public IQueryable<AppTask> Sort(IQueryable<AppTask> query, string ascOrDesc)
    {
        if (ascOrDesc == "desc")
            return query.OrderByDescending(t => t.Priority);
        return query.OrderBy(t => t.Priority);
    }
}