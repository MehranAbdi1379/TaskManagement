﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Shared.DTOs.Task
{
    public class AsignTaskToUserDto
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
