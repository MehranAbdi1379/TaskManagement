using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;

namespace TaskManagement.Repository.Repositories
{
    public interface ITaskRepository: IBaseRepository<AppTask>
    {
        Task<PagedResult<TaskResponseDto>> GetTasksAsync(TaskQueryParameters queryParams);
    }
}