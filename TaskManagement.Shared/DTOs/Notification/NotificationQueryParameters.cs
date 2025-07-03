namespace TaskManagement.Shared.DTOs.Notification;

public class NotificationQueryParameters
{
    public int PageNumber { get; set; } = 1; // Default to first page
    public int PageSize { get; set; } = 10; // Default page size
    public string SortOrder { get; set; } = "desc"; // "asc" or "desc"
    public bool History { get; set; }
}