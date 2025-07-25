using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Filters;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs;
using TaskManagement.Shared.DTOs.Task;
using TaskManagement.Shared.ServiceInterfaces;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Service.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository baseRepository;
    private readonly IMapper mapper;
    private readonly IBaseRepository<BaseNotification> notificationRepository;
    private readonly INotificationService notificationService;
    private readonly ITaskAssignmentRequestRepository taskAssignmentRequestRepository;
    private readonly IUserContext userContext;
    private readonly UserManager<ApplicationUser> userManager;


    public TaskService(ITaskRepository baseRepository, IMapper mapper, IUserContext userContext,
        UserManager<ApplicationUser> userManager,
        ITaskAssignmentRequestRepository taskAssignmentRequestRepository, INotificationService notificationService,
        IBaseRepository<BaseNotification> notificationRepository)
    {
        this.baseRepository = baseRepository;
        this.mapper = mapper;
        this.userContext = userContext;
        this.userManager = userManager;
        this.taskAssignmentRequestRepository = taskAssignmentRequestRepository;
        this.notificationService = notificationService;
        this.notificationRepository = notificationRepository;
    }


    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto)
    {
        var task = new AppTask(dto.Title, dto.Description, dto.TaskStatus, userContext.UserId);
        if (dto.Priority != 0) task.SetPriority(dto.Priority);
        if (dto.DueDate.HasValue) task.SetDueDate(dto.DueDate.Value);
        await baseRepository.AddAsync(task);
        var mappedResult = mapper.Map<TaskResponseDto>(task);
        mappedResult.IsOwner = true;
        return mappedResult;
    }

    public async Task<TaskResponseDto> UpdateTaskAsync(UpdateTaskDto dto, int taskId)
    {
        var task = await baseRepository.GetTaskByIdAsync(taskId);
        task.SetStatus(dto.TaskStatus);
        task.SetTitle(dto.Title);
        task.SetDescription(dto.Description);
        task.UpdateUpdatedAt();
        if (dto.Priority != 0) task.SetPriority(dto.Priority);
        if (dto.DueDate.HasValue) task.SetDueDate(dto.DueDate.Value);

        await baseRepository.UpdateAsync(task);
        var result = mapper.Map<TaskResponseDto>(task);
        result.IsOwner = task.OwnerId == userContext.UserId;
        return result;
    }

    public async Task<PagedResult<TaskResponseDto>> GetAllTasksAsync(TaskQueryParameters parameters)
    {
        var (tasks, totalCount) = await baseRepository.GetTasksAsync(parameters.PageNumber, parameters.PageSize,
            parameters.Status, parameters.Priority, parameters.DueDate,
            parameters.SortOptions, parameters.Desc);
        var mappedResults = mapper.Map<List<TaskResponseDto>>(tasks);

        var mappedDict = mappedResults.ToDictionary(mr => mr.Id);

        foreach (var task in tasks)
            mappedDict[task.Id].IsOwner = task.OwnerId == userContext.UserId;

        return new PagedResult<TaskResponseDto>
        {
            Items = mappedResults,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };
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

    public async Task<TaskResponseDto> UpdateTaskStatusAsync(UpdateTaskStatusDto dto, int taskId)
    {
        var task = await baseRepository.GetTaskByIdAsync(taskId);
        task.SetStatus(dto.TaskStatus);
        task.UpdateUpdatedAt();
        await baseRepository.UpdateAsync(task);
        var result = mapper.Map<TaskResponseDto>(task);
        result.IsOwner = task.OwnerId == userContext.UserId;
        return result;
    }


    public async Task<BaseNotification> RequestTaskAssignmentAsync(string assigneeEmail, int taskId)
    {
        var task = await baseRepository.GetTaskByIdAsync(taskId);

        var assignee = await userManager.Users.FirstOrDefaultAsync(u => u.Email == assigneeEmail);
        if (assignee == null) throw new Exception($"User with email {assigneeEmail} does not exist to assign.");

        if (await taskAssignmentRequestRepository.RequestAlreadyExists(assignee.Id, taskId))
            throw new Exception("This request already exists.");

        var taskUsers = task.TaskAssignmentRequests;
        if (assignee.Id == userContext.UserId || taskUsers.Any(x => x.AssigneeId == userContext.UserId))
            throw new Exception($"User with email {assigneeEmail} already has the task with id {taskId}.");

        var owner = await userManager.Users.FirstAsync(x => x.Id == userContext.UserId);

        var request = new TaskAssignmentRequest(assignee.Id, taskId);


        // Create a notification for the assignee
        var notification = await notificationService.CreateNotification(assignee.Id, "Task Assignment Request",
            $"You have been invited to join Task (Title: {task.Title}, Description: {task.Description}) by ({owner.FirstName + " " + owner.LastName}). Accept or Reject."
            , NotificationType.TaskAssignmentRequest
        );

        request.RequestNotification = notification;
        await taskAssignmentRequestRepository.AddAsync(request);

        return notification;
    }

    public async Task<BaseNotification> RespondToTaskAssignmentAsync(int notificationId, bool accept)
    {
        var request =
            await taskAssignmentRequestRepository.GetTaskAssignmentRequestByNotificationIdAsync(notificationId);

        request.SetIsAccepted(accept);
        if (!accept) request.Delete();

        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == request.AssigneeId);
        if (user == null) throw new Exception($"User with id {request.AssigneeId} does not exist to assign.");

        request.RequestNotification.ReadNotification();
        await taskAssignmentRequestRepository.UpdateAsync(request);

        var taskOwnerId = request.Task.OwnerId;

        if (accept)
            // Notify the task owner
            return await notificationService.CreateNotification(
                taskOwnerId, "Task Assignment Accepted",
                $"User ({user.FirstName + " " + user.LastName}) has accepted your task invitation for task" +
                $" (Title: {request.Task.Title}, Description: {request.Task.Description})."
                , NotificationType.General
            );

        return await notificationService.CreateNotification(
            taskOwnerId, "Task Assignment Rejected",
            $"User {user.FirstName + " " + user.LastName} has rejected your task invitation." +
            $" (Title: {request.Task.Title}, Description: {request.Task.Description})."
            , NotificationType.General
        );
    }

    public async Task<List<TaskAssignedUserResponseDto>> GetTaskAssignedUsers(int taskId)
    {
        var users = await baseRepository.GetTaskAssignedUsersAsync(taskId);
        return mapper.Map<List<TaskAssignedUserResponseDto>>(users);
    }

    public async Task<BaseNotification> UnassignTaskAsync(int taskId, int userId)
    {
        var request =
            await taskAssignmentRequestRepository.GetTaskAssignmentRequestByUserIdAndTaskIdAsync(userId, taskId);
        request.SetIsAccepted(false);
        request.Delete();
        var task = await baseRepository.GetTaskByIdAsync(taskId);
        await taskAssignmentRequestRepository.UpdateAsync(request);
        return await notificationService.CreateNotification(
            userId, "Task Unassignment",
            $"You are unassigned from Task (Title: {task.Title}, Description: {task.Description})"
            , NotificationType.General
        );
    }

    public async Task<TaskReportResponseDto> GetTaskReport(TaskReportQueryDto dto)
    {
        var filters = new TaskReportFilters
        {
            CreatedEndDate = dto.CreatedEndDate,
            CreatedStartDate = dto.CreatedStartDate,
            DueDateEndDate = dto.DueDateEndDate,
            DueDateStartDate = dto.DueDateStartDate,
            UpdatedEndDate = dto.UpdatedEndDate,
            UpdatedStartDate = dto.UpdatedStartDate
        };
        var report = new TaskReportResponseDto();
        var priorities = new List<TaskReportResponseDto.TaskPriorityCount>();
        priorities.Add(new TaskReportResponseDto.TaskPriorityCount
        {
            Count = await baseRepository.GetTaskCountByPriorityAsync(TaskPriority.Low, filters),
            Priority = nameof(TaskPriority.Low)
        });
        priorities.Add(new TaskReportResponseDto.TaskPriorityCount
        {
            Count = await baseRepository.GetTaskCountByPriorityAsync(TaskPriority.Medium, filters),
            Priority = nameof(TaskPriority.Medium)
        });
        priorities.Add(new TaskReportResponseDto.TaskPriorityCount
        {
            Count = await baseRepository.GetTaskCountByPriorityAsync(TaskPriority.High, filters),
            Priority = nameof(TaskPriority.High)
        });
        report.ByPriority = priorities;

        var statuses = new List<TaskReportResponseDto.TaskStatusCount>();
        statuses.Add(new TaskReportResponseDto.TaskStatusCount
        {
            Count = await baseRepository.GetTaskCountByStatusAsync(TaskStatus.Pending, filters),
            Status = nameof(TaskStatus.Pending)
        });
        statuses.Add(new TaskReportResponseDto.TaskStatusCount
        {
            Count = await baseRepository.GetTaskCountByStatusAsync(TaskStatus.InProgress, filters),
            Status = nameof(TaskStatus.InProgress)
        });
        statuses.Add(new TaskReportResponseDto.TaskStatusCount
        {
            Count = await baseRepository.GetTaskCountByStatusAsync(TaskStatus.Completed, filters),
            Status = nameof(TaskStatus.Completed)
        });
        statuses.Add(new TaskReportResponseDto.TaskStatusCount
        {
            Count = await baseRepository.GetTaskCountByStatusAsync(TaskStatus.Cancelled, filters),
            Status = nameof(TaskStatus.Cancelled)
        });
        report.ByStatus = statuses;

        report.Total = await baseRepository.GetTotalTaskCountAsync(filters);
        report.OverDue = await baseRepository.GetOverDueCountAsync(filters);
        report.DueToday = await baseRepository.GetDueTodayCountAsync(filters);
        report.Deleted = await baseRepository.GetDeletedTaskCountAsync(filters);

        return report;
    }
}