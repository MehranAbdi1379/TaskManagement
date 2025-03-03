using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Repository;
using TaskManagement.Repository.Repositories;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Service.Interfaces;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Service.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository baseRepository;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;

        public TaskService(ITaskRepository baseRepository, IMapper mapper, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.baseRepository = baseRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.userManager = userManager;
        }

        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto)
        {
            var task = mapper.Map<AppTask>(dto);
            task.SetOwnerId(userContext.UserId);
            await baseRepository.AddAsync(task);
            return mapper.Map<TaskResponseDto>(task);
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(UpdateTaskDto dto)
        {
            var task = await baseRepository.GetTaskByIdAsync(dto.Id);
            task.SetStatus(dto.Status);
            task.SetTitle(dto.Title);
            task.SetDescription(dto.Description);
            task.UpdatedAt = DateTime.Now;
            await baseRepository.UpdateAsync(task);
            return mapper.Map<TaskResponseDto>(task);
        }

        public async Task<PagedResult<TaskResponseDto>> GetAllTasksAsync(TaskQueryParameters parameters)
        {
            return await baseRepository.GetTasksAsync(parameters);
        }

        public async Task<TaskResponseDto> GetTaskByIdAsync(int id)
        {
            var task = await baseRepository.GetTaskByIdAsync(id);
            return mapper.Map<TaskResponseDto>(task);
        }

        public async Task DeleteTaskByIdAsync(int id)
        {
            await baseRepository.DeleteTaskAsync(id);
        }

        public async Task<TaskResponseDto> UpdateTaskStatusAsync(UpdateTaskStatusDto dto)
        {
            var task = await baseRepository.GetTaskByIdAsync(dto.Id);
            task.SetStatus(dto.Status);
            task.UpdatedAt = DateTime.Now;
            await baseRepository.UpdateAsync(task);
            return mapper.Map<TaskResponseDto>(task);
        }

        
    }
}
