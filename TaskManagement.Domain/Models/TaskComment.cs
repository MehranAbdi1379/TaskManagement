using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models
{
    public class TaskComment: BaseEntity
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
