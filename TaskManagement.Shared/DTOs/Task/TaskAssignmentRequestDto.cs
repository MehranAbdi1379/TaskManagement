using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Shared.DTOs.Task
{
    public class TaskAssignmentRequestDto
    {
        public string AssigneeEmail { get; set; }
        public int TaskId { get; set; }
    }
}
