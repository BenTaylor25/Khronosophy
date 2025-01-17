using SchedulingDemo.Models;

namespace SchedulingDemo.Services;

public static class LoadSampleData
{
    public static User GetUserWithTasks()
    {
        User user = new();

        user.Taskboard.Tasks = [
            new TaskboardTask("test 1", TimeSpan.FromHours(3)),
            new TaskboardTask("test 2", TimeSpan.FromHours(4)),
            new TaskboardTask("test 3", TimeSpan.FromHours(2)),
            new TaskboardTask("test 4", TimeSpan.FromHours(6)),
            new TaskboardTask("test 5", TimeSpan.FromHours(3)),
            new TaskboardTask("test 6", TimeSpan.FromHours(1)),
            new TaskboardTask("test 7", TimeSpan.FromHours(3)),
            new TaskboardTask("test 8", TimeSpan.FromHours(2)),
            new TaskboardTask("test 9", TimeSpan.FromHours(2)),
        ];

        return user;
    }
}
