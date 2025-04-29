using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
using TaskManagement.Shared.DTOs.Task;
using TaskManagement.Shared.DTOs.TaskComment;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Service.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository baseRepository;
        private readonly IBaseRepository<AppTaskUser> taskUserRepository;
        private readonly ITaskAssignmentRequestRepository taskAssignmentRequestRepository;
        private readonly IBaseRepository<BaseNotification> notificationRepository;
        private readonly INotificationService notificationService;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;
        


        public TaskService(ITaskRepository baseRepository, IMapper mapper, IUserContext userContext, UserManager<ApplicationUser> userManager, IBaseRepository<AppTaskUser> taskUserRepository, ITaskAssignmentRequestRepository taskAssignmentRequestRepository, INotificationService notificationService, IBaseRepository<BaseNotification> notificationRepository)
        {
            this.baseRepository = baseRepository;
            this.mapper = mapper;
            this.userContext = userContext;
            this.userManager = userManager;
            this.taskUserRepository = taskUserRepository;
            this.taskAssignmentRequestRepository = taskAssignmentRequestRepository;
            this.notificationService = notificationService;
            this.notificationRepository = notificationRepository;
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
            var result = mapper.Map<TaskResponseDto>(task);
            result.IsOwner = task.OwnerId == userContext.UserId;
            return result;
        }

        public async Task<PagedResult<TaskResponseDto>> GetAllTasksAsync(TaskQueryParameters parameters)
        {
            return await baseRepository.GetTasksAsync(parameters);
        }

        public async Task<TaskResponseDto> GetTaskByIdAsync(int id)
        {
            var task = await baseRepository.GetTaskByIdAsync(id);
            var result = mapper.Map<TaskResponseDto>(task);
            result.IsOwner = task.OwnerId == userContext.UserId;
            return result;
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
            var result = mapper.Map<TaskResponseDto>(task);
            result.IsOwner = task.OwnerId == userContext.UserId;
            return result;
        }

        private async Task AssignTaskToUserAsync(int taskId)
        {
            var appTaskUser = new AppTaskUser(userContext.UserId, taskId);
            await taskUserRepository.AddAsync(appTaskUser);
        }


        public async Task<BaseNotification> RequestTaskAssignmentAsync(string assigneeEmail, int taskId)
        {
            var task = await baseRepository.GetTaskByIdAsync(taskId);

            var assignee = await userManager.Users.FirstOrDefaultAsync(u => u.Email == assigneeEmail);
            if (assignee == null) throw new Exception("User does not exist to assign.");

            if (await taskAssignmentRequestRepository.RequestAlreadyExists(assignee.Id, taskId)) throw new Exception("This request already exists.");

            var taskUsers = (await taskUserRepository.GetAllAsync()).Where(x => x.TaskId == taskId);
            if (assignee.Id == userContext.UserId || taskUsers.Any(x => x.UserId == userContext.UserId)) throw new Exception("User already has the task.");

            var owner = await userManager.Users.FirstAsync(x => x.Id == userContext.UserId);

            var request = new TaskAssignmentRequest(userContext.UserId,assignee.Id, taskId);

            await taskAssignmentRequestRepository.AddAsync(request);

            // Create a notification for the assignee
            var notification = await notificationService.CreateNotification(assignee.Id,"Task Assignment Request",
                $"You have been invited to join Task (Title: {task.Title}, Description: {task.Description}) by ({owner.FirstName + " " + owner.LastName}). Accept or Reject."
                ,NotificationType.TaskAssignmentRequest
            );

            request.SetRequestNotificationId(notification.Id);
            await taskAssignmentRequestRepository.UpdateAsync(request);

            return notification;
        }

        public async Task<BaseNotification> RespondToTaskAssignmentAsync(int notificationId, bool accept)
        {
            var request = await taskAssignmentRequestRepository.GetTaskAssignmentRequestByNotificationIdAsync(notificationId);
            if (request == null) throw new Exception("Task Assignment Request does not exist.");

            request.SetIsAccepted(accept);
            request.Deleted = true;

            await taskAssignmentRequestRepository.UpdateAsync(request);
            var user = userManager.Users.First(u => u.Id == request.AssigneeId);
            var task = await baseRepository.GetByIdAsync(request.TaskId);

            var notification = await notificationRepository.GetByIdAsync(request.RequestNotificationId);
            notification.SetIsRead(true);
            await notificationRepository.UpdateAsync(notification);

            if (accept)
            {

                await AssignTaskToUserAsync(request.TaskId);
                
                // Notify the task owner
                return await notificationService.CreateNotification(
                    request.TaskOwnerId, "Task Assignment Accepted",
                    $"User ({user.FirstName + " " + user.LastName}) has accepted your task invitation for task" +
                    $"(Title: {task.Title}, Description: {task.Description})."
                    ,NotificationType.General
                );
            }
            else
            {
                return await notificationService.CreateNotification(
                    request.TaskOwnerId,"Task Assignment Rejected",
                    $"User {user.FirstName + " " + user.LastName} has rejected your task invitation." +
                    $"(Title: {task.Title}, Description: {task.Description})."
                    , NotificationType.General
                );
            }
        }

        public async Task<List<TaskAssignedUserResponseDto>> GetTaskAssignedUsers(int taskId)
        {
            return await baseRepository.GetTaskAssignedUsersAsync(taskId);
        }

        public async Task<BaseNotification> UnassignTaskAsync(int taskId, int userId)
        {
            var task = await baseRepository.GetTaskByIdAsync(taskId);
            await baseRepository.UnassignTaskAsync(taskId, userId);
            return await notificationService.CreateNotification(
                userId, "Task Unassignment",
                $"You are unassigned from Task (Title: {task.Title}, Description: {task.Description})" 
                ,NotificationType.General
            );
        }
    }
}
