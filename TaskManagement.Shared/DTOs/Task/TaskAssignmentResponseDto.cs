using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Shared.DTOs.Task
{
    public class TaskAssignmentResponseDto
    {
        public int RequestNotificationId { get; set; }
        public bool Accept { get; set; }
    }
}
