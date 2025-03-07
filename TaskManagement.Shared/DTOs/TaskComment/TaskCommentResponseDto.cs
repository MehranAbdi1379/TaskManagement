using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Shared.DTOs.TaskComment
{
    public class TaskCommentResponseDto
    {
        public int Id { get; set; }
        public string UserFullName { get; set; }
        public int TaskId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
