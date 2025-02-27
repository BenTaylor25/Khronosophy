
using Calendar.Models;
using ErrorOr;

namespace Calendar.Services.Taskboard;

public class TaskboardService : ITaskboardService
{
    public ErrorOr<List<TaskboardTask>> GetUserTasks(KhronosophyUser user)
    {
        return user.Taskboard.Tasks;
    }

    public ErrorOr<Success> AddTaskToUser(
        KhronosophyUser user,
        TaskboardTask task
    )
    {
        user.Taskboard.Tasks.Add(task);
        return new Success();
    }
}
