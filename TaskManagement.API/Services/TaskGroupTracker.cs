namespace TaskManagement.API.Services;

public class TaskGroupTracker
{
    public List<(int userId,int taskId)> TaskUserGroups { get; private set; } = [];

    public void AddTaskUserGroup(int userId, int taskId)
    {
        if(TaskUserGroups.Count == 0 || !TaskUserGroups.Any(x => x.userId == userId && x.taskId == taskId)) TaskUserGroups.Add(new (userId,taskId));
    }

    public void RemoveTaskUserGroup(int userId, int taskId)
    {
        if(TaskUserGroups.Any(x => x.userId == userId && x.taskId == taskId)) TaskUserGroups.Remove((userId, taskId));
    }
}