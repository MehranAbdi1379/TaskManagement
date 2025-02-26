using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Service.DTOs.Task
{
    public class UpdateTaskStatusDto
    {
        public int Id { get; set; }
        public Status Status { get; set; }
    }
}
