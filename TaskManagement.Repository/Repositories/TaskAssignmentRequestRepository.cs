using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Repository.Repositories
{
    public class TaskAssignmentRequestRepository : BaseRepository<TaskAssignmentRequest>, ITaskAssignmentRequestRepository
    {
        private readonly IUserContext userContext;
        public TaskAssignmentRequestRepository(TaskManagementDBContext context, IUserContext userContext) : base(context)
        {
            this.userContext = userContext;
        }

        public async Task<TaskAssignmentRequest> GetTaskAssignmentRequestByNotificationIdAsync(int notificationId)
        {
            return await _dbSet.FirstAsync(x => x.RequestNotificationId == notificationId);
        }

        public async Task<bool> RequestAlreadyExists(int assigneeId, int taskId)
        {
            return await _dbSet.AnyAsync(x => x.AssigneeId == assigneeId && x.TaskId == taskId && x.TaskOwnerId == userContext.UserId && x.Deleted == false);
        }
    }
}
