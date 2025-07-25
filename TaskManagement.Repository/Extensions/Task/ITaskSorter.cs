using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task.Enums;

namespace TaskManagement.Repository.Extensions.Task;

public interface ITaskSorter
{
    public TaskSortOptions SortOption { get; }
    public IQueryable<AppTask> Sort(IQueryable<AppTask> query, bool desc);
}