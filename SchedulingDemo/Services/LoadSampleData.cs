using SchedulingDemo.Models;

namespace SchedulingDemo.Services;

public static class LoadSampleData
{
    public static User GetUserWithTasks()
    {
        User user = new User();

        user.Taskboard = new Taskboard();
        user.Calendar = new Calendar();

        user.Taskboard.Tasks = [
            new TaskboardTask("test 1", 4)
        ];

        return user;
    }
}
