﻿using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Repository.Repositories
{
    public interface ITaskRepository: IBaseRepository<AppTask>
    {
        Task<PagedResult<TaskResponseDto>> GetTasksAsync(TaskQueryParameters queryParams);
        Task<AppTask> GetTaskByIdAsync(int id);
        Task DeleteTaskAsync(int id);
        Task<List<TaskAssignedUserResponseDto>> GetTaskAssignedUsersAsync(int taskId);

        Task UnassignTaskAsync(int taskId, int userId);
    }
}