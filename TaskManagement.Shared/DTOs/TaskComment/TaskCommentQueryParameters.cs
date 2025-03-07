﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Shared.DTOs.TaskComment
{
    public class TaskCommentQueryParameters
    {
        public int PageNumber { get; set; } = 1;  // Default to first page
        public int PageSize { get; set; } = 10;   // Default page size
        public string SortOrder { get; set; } = "asc"; // "asc" or "desc"
        public bool CommentOwner { get; set; } = false;
    }
}
