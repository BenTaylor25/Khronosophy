using SchedulingDemo.Models;

namespace SchedulingDemo.Services;

public static class LoadSampleData
{
    public static User GetUserWithTasks()
    {
        User user = new();

        user.Taskboard.Tasks = [
            new TaskboardTask("test 1", TimeSpan.FromHours(3)),
            new TaskboardTask("test 2", TimeSpan.FromHours(4))
        ];

        return user;
    }
}
