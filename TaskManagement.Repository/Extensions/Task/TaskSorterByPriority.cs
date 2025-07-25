using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task.Enums;

namespace TaskManagement.Repository.Extensions.Task;

public class TaskSorterByPriority : ITaskSorter
{
    public TaskSortOptions SortOption => TaskSortOptions.Priority;

    public IQueryable<AppTask> Sort(IQueryable<AppTask> query, bool desc)
    {
        if (desc)
            return query.OrderByDescending(x => x.Priority);
        return query.OrderBy(x => x.Priority);
    }
}