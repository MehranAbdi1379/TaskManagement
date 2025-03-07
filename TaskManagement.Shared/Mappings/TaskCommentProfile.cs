using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.Shared.Mappings
{
    public class TaskCommentProfile: Profile
    {
        public TaskCommentProfile()
        {
            CreateMap<TaskCommentResponseDto, TaskComment>();
        }
    }
}
