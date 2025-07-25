using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task.Enums;

namespace TaskManagement.Repository.Extensions.Task;

public static class TaskSorter
{
    private static readonly List<ITaskSorter> Sorters = new()
    {
        new TaskSorterByPriority(),
        new TaskSorterByDueDate(),
        new TaskSorterByUpdatedDate()
    };

    public static IQueryable<AppTask> SortTasks(this IQueryable<AppTask> query, TaskSortOptions option,
        bool desc)
    {
        var sorter = Sorters.FirstOrDefault(s => s.SortOption == option);
        if (sorter == null) throw new Exception($"Sorter not found: {option}");
        return sorter.Sort(query, desc);
    }
}