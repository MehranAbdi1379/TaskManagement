namespace TaskManagement.Shared.ServiceInterfaces;

public class TaskReportResponseDto
{
    public int Total { get; set; }
    public List<TaskPriorityCount> ByPriority { get; set; }
    public List<TaskStatusCount> ByStatus { get; set; }
    public int OverDue { get; set; }
    public int DueToday { get; set; }
    public int Deleted { get; set; }


    public class TaskStatusCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class TaskPriorityCount
    {
        public string Priority { get; set; }
        public int Count { get; set; }
    }
}