namespace TaskManagement.Domain.Filters;

public class TaskReportFilters
{
    public DateTime? CreatedStartDate { get; set; }
    public DateTime? CreatedEndDate { get; set; }
    public DateTime? UpdatedStartDate { get; set; }
    public DateTime? UpdatedEndDate { get; set; }
    public DateTime? DueDateStartDate { get; set; }
    public DateTime? DueDateEndDate { get; set; }
}