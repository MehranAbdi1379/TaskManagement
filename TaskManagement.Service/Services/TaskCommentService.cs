using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Repository.Repositories;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Service.DTOs;
using TaskManagement.Shared.ServiceInterfaces;
using TaskManagement.Repository;
using TaskManagement.Shared.DTOs.TaskComment;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.Service.Hubs;

namespace TaskManagement.Service.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly ITaskCommentRepository baseRepository;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationHub> hubContext;

        public TaskCommentService(ITaskCommentRepository baseRepository, IMapper mapper, IUserContext userContext, UserManager<ApplicationUser> userManager, IHubContext<NotificationHub> hubContext)
        {
            this.baseRepository = baseRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public async Task<TaskCommentResponseDto> CreateTaskCommentAsync(CreateTaskCommentDto dto, int taskId)
        {
            var taskComment = new TaskComment(taskId, userContext.UserId, dto.Text);
            await baseRepository.AddTaskCommentAsync(taskComment);
            var result = mapper.Map<TaskCommentResponseDto>(taskComment);
            var user = userManager.Users.First(u => u.Id == userContext.UserId);
            result.UserFullName = user.FirstName + " " + user.LastName;
            result.IsOwner = true;
            await NotifyTaskUsersAsync(result.TaskId, result);
            return result;
        }

        public async Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsByTaskIdAsync(TaskCommentQueryParameters parameters, int taskId)
        {
            if(parameters.CommentOwner == false) return await baseRepository.GetTaskCommentsAsync(parameters, taskId);
            else return await baseRepository.GetTaskCommentsByTaskAndUserIdAsync(parameters, taskId);
        }

        public async Task NotifyTaskUsersAsync(int taskId, TaskCommentResponseDto comment)
        {
            await hubContext.Clients.Group($"task-{taskId}").SendAsync("ReceiveTaskComment", comment);
        }

        //public async Task<TaskResponseDto> GetTaskCommentByIdAsync(int id)
        //{
        //    var task = await baseRepository.GetByIdAsync(id);
        //    return mapper.Map<TaskResponseDto>(task);
        //}

        public async Task DeleteTaskCommentByIdAsync(int id)
        {
            await baseRepository.DeleteTaskCommentAsync(id);
        }
    }
}
