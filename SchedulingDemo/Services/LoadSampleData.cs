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

    public static User GetUserWithTasksAndIntensities()
    {
        User user = new()
        {
            DailyIntensityCapacity = 20
        };

        user.Taskboard.Tasks = [
            new TaskboardTask("test 1", TimeSpan.FromHours(3), 3, 8),
            new TaskboardTask("test 2", TimeSpan.FromHours(4), 2, 7),
            new TaskboardTask("test 3", TimeSpan.FromHours(2), 1, 10),
            new TaskboardTask("test 4", TimeSpan.FromHours(6), 10, 9),
            new TaskboardTask("test 5", TimeSpan.FromHours(3), 4, 9),
            new TaskboardTask("test 6", TimeSpan.FromHours(1), 6, 10),
            new TaskboardTask("test 7", TimeSpan.FromHours(3), 7, 7),
            new TaskboardTask("test 8", TimeSpan.FromHours(2), 5, 2),
            new TaskboardTask("test 9", TimeSpan.FromHours(2), 9, 8),
        ];

        return user;
    }
}
