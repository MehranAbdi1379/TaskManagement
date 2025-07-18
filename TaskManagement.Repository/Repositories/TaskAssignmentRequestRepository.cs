﻿using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories;

public class TaskAssignmentRequestRepository : BaseRepository<TaskAssignmentRequest>, ITaskAssignmentRequestRepository
{
    private readonly IUserContext userContext;

    public TaskAssignmentRequestRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
    {
        this.userContext = userContext;
    }

    public async Task<TaskAssignmentRequest> GetTaskAssignmentRequestByNotificationIdAsync(int notificationId)
    {
        var taskAssignmentRequest =
            await _dbSet
                .Include(t => t.RequestNotification)
                .Include(t => t.Task)
                .FirstOrDefaultAsync(x => x.RequestNotificationId == notificationId && x.Deleted == false);
        if (taskAssignmentRequest == null)
            throw new InvalidOperationException(
                $"TaskAssignmentRequest for notification with id {notificationId} not found");
        return taskAssignmentRequest;
    }

    public async Task<bool> RequestAlreadyExists(int assigneeId, int taskId)
    {
        return await _dbSet.AnyAsync(x =>
            x.AssigneeId == assigneeId && x.TaskId == taskId &&
            x.Deleted == false);
    }

    public async Task<TaskAssignmentRequest> GetTaskAssignmentRequestByUserIdAndTaskIdAsync(int userId, int taskId)
    {
        var taskAssignmentRequest = await
            _dbSet.FirstOrDefaultAsync(tar => tar.TaskId == taskId && tar.AssigneeId == userId && tar.Deleted == false);
        if (taskAssignmentRequest == null)
            throw new Exception($"Task assignment request not found for user {userId} and task {taskId}");
        return taskAssignmentRequest;
    }
}