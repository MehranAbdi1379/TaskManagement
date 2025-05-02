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

namespace TaskManagement.Service.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly ITaskCommentRepository baseRepository;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBaseRepository<CommentNotification> commentNotificationRepository;

        public TaskCommentService(ITaskCommentRepository baseRepository, IMapper mapper, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.baseRepository = baseRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.userManager = userManager;
        }

        public async Task<(TaskCommentResponseDto,List<BaseNotification>)> CreateTaskCommentAsync(CreateTaskCommentDto dto, int taskId, List<(int userId,int taskId)> taskUserGroups)
        {
            var taskComment = new TaskComment(taskId, userContext.UserId, dto.Text);
            var (newTaskComment, baseNotifications) = await baseRepository.AddTaskCommentAsync(taskComment, taskUserGroups);
            taskComment = newTaskComment;

            foreach (var baseNotification in baseNotifications)
            {
                var commentNotification = new CommentNotification(baseNotification.Id, taskComment.Id);
                await commentNotificationRepository.AddAsync(commentNotification);
            }
            
            var result = mapper.Map<TaskCommentResponseDto>(taskComment);
            var user = userManager.Users.First(u => u.Id == userContext.UserId);
            result.UserFullName = user.FirstName + " " + user.LastName;
            result.IsOwner = true;
            return (result,baseNotifications);
        }

        public async Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsByTaskIdAsync(TaskCommentQueryParameters parameters, int taskId)
        {
            if(parameters.CommentOwner == false) return await baseRepository.GetTaskCommentsAsync(parameters, taskId);
            return await baseRepository.GetTaskCommentsByTaskAndUserIdAsync(parameters, taskId);
        }


        //public async Task<TaskResponseDto> GetTaskCommentByIdAsync(int id)
        //{
        //    var task = await baseRepository.GetByIdAsync(id);
        //    return mapper.Map<TaskResponseDto>(task);
        //}

        public async Task<TaskComment> DeleteTaskCommentByIdAsync(int id)
        {
            var taskComment = await baseRepository.GetByIdAsync(id);
            await baseRepository.DeleteTaskCommentAsync(id);
            return taskComment;
        }
    }
}
