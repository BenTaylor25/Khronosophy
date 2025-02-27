
using Calendar.Models;
using ErrorOr;

namespace Calendar.Services.Taskboard;

public class TaskboardService : ITaskboardService
{
    public ErrorOr<List<TaskboardTask>> GetUserTasks(KhronosophyUser user)
    {
        return user.Taskboard.Tasks;
    }
}
