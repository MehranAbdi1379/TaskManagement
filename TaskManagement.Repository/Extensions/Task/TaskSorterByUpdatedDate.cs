using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task.Enums;

namespace TaskManagement.Repository.Extensions.Task;

public class TaskSorterByUpdatedDate : ITaskSorter
{
    public TaskSortOptions SortOption => TaskSortOptions.UpdatedDate;

    public IQueryable<AppTask> Sort(IQueryable<AppTask> query, string ascOrDesc)
    {
        if (ascOrDesc == "desc")
            return query.OrderByDescending(t => t.UpdatedAt);
        return query.OrderBy(t => t.UpdatedAt);
    }
}