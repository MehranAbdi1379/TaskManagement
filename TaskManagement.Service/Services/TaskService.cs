using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Repository;
using TaskManagement.Service.Interfaces;

namespace TaskManagement.Service.Services
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<AppTask> baseRepository;
        public TaskService(IBaseRepository<AppTask> baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public async Task<AppTask> CreateTaskAsync(AppTask entity)
        {
            return await baseRepository.AddAsync(entity);
        }

        public async Task<AppTask> UpdateTaskAsync(AppTask entity)
        {
            return await baseRepository.UpdateAsync(entity);
        }

        public async Task<List<AppTask>> GetAllTasksAsync()
        {
            return (await baseRepository.GetAllAsync()).ToList();
        }

        public async Task<AppTask> GetTaskByIdAsync(int id)
        {
            return await baseRepository.GetByIdAsync(id);
        }

        public async Task DeleteTaskByIdAsync(int id)
        {
            await baseRepository.DeleteAsync(id);
        }
    }
}
