using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models
{
    public class TaskAssignmentNotification: BaseEntity
    {
        public int NotificationId { get; set; }
        public bool Accepted { get; set; }
        public int RequestId { get; set; }
    }
}
