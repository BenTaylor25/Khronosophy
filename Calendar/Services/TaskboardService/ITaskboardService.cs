using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.TaskboardService;

public interface ITaskboardService
{
    ErrorOr<List<TaskboardTask>> GetUserTasks(KhronosophyUser user);
    ErrorOr<Success> AddTaskToUser(
        KhronosophyUser user,
        TaskboardTask task
    );
}
