using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs;
using TaskManagement.Shared.DTOs.TaskComment;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Service.Services;

public class TaskCommentService : ITaskCommentService
{
    private readonly ITaskCommentRepository baseRepository;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;
    private readonly UserManager<ApplicationUser> userManager;

    public TaskCommentService(ITaskCommentRepository baseRepository, IMapper mapper, IUserContext userContext,
        UserManager<ApplicationUser> userManager)
    {
        this.baseRepository = baseRepository;
        this.mapper = mapper;
        this.userContext = userContext;
        this.userManager = userManager;
    }

    public async Task<(TaskCommentResponseDto, List<BaseNotification>)> CreateTaskCommentAsync(CreateTaskCommentDto dto,
        int taskId, List<(int userId, int taskId)> taskUserGroups)
    {
        var taskComment = new TaskComment(taskId, userContext.UserId, dto.Text);
        var (newTaskComment, baseNotifications) = await baseRepository.AddTaskCommentAsync(taskComment, taskUserGroups);
        taskComment = newTaskComment;

        foreach (var baseNotification in baseNotifications) taskComment.Notifications.Add(baseNotification);

        await baseRepository.UpdateAsync(taskComment);

        var result = mapper.Map<TaskCommentResponseDto>(taskComment);
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userContext.UserId);
        if (user == null) throw new Exception($"User not found with id {userContext.UserId}");

        result.UserFullName = user.FirstName + " " + user.LastName;
        result.IsOwner = true;
        return (result, baseNotifications);
    }

    public async Task<PagedResult<TaskCommentResponseDto>> GetTaskCommentsByTaskIdAsync(
        TaskCommentQueryParameters parameters, int taskId)
    {
        if (parameters.CommentOwner == false)
        {
            var (taskComments, totalCount) = await baseRepository.GetTaskCommentsAsync(parameters.PageNumber,
                parameters.PageSize, parameters.SortOrder, taskId, false);

            var mappedResults = mapper.Map<List<TaskCommentResponseDto>>(taskComments);

            var mappedDict = mappedResults.ToDictionary(mr => mr.Id);

            foreach (var comment in taskComments)
                mappedDict[comment.Id].IsOwner = comment.UserId == userContext.UserId;


            var userIds = taskComments.Select(tc => tc.UserId).ToHashSet();

            var usersDict = userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id);

            foreach (var comment in taskComments)
            {
                if (!usersDict.TryGetValue(comment.UserId, out var user))
                    throw new InvalidOperationException($"User with ID '{comment.UserId}' not found.");

                mappedDict[comment.Id].UserFullName = $"{user.FirstName} {user.LastName}";
            }

            return new PagedResult<TaskCommentResponseDto>
            {
                Items = mappedResults,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        else
        {
            var (taskComments, totalCount) =
                await baseRepository.GetTaskCommentsAsync(parameters.PageSize, parameters.PageNumber,
                    parameters.SortOrder, taskId, true);

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userContext.UserId);
            if (user == null) throw new InvalidOperationException($"User with ID '{userContext.UserId}' not found.");

            var mappedResults = mapper.Map<List<TaskCommentResponseDto>>(taskComments);
            foreach (var comment in mappedResults)
            {
                comment.UserFullName = $"{user.FirstName} {user.LastName}";
                comment.IsOwner = true;
            }

            return new PagedResult<TaskCommentResponseDto>
            {
                Items = mappedResults,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
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