using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models
{
    public class AppTaskUser: BaseEntity
    {
        public int UserId { get; private set; }
        public int TaskId { get; private set; }

        public AppTaskUser()
        {
            
        }

        public AppTaskUser(int userId, int taskId)
        {
            SetUserId(userId);
            SetTaskId(taskId);
        }

        public void SetUserId(int userId)
        {
            UserId = userId;
        }

        public void SetTaskId(int taskId)
        {   
            TaskId = taskId;
        }
    }
}
