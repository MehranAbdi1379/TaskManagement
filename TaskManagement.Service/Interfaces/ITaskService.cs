using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;

namespace TaskManagement.Service.Interfaces
{
    public interface ITaskService
    {
        Task<AppTask> CreateTaskAsync(AppTask entity);
        Task DeleteTaskByIdAsync(int id);
        Task<List<AppTask>> GetAllTasksAsync();
        Task<AppTask> GetTaskByIdAsync(int id);
        Task<AppTask> UpdateTaskAsync(AppTask entity);
        Task<AppTask> UpdateTaskStatusAsync(int id, Status taskStatus);
    }
}