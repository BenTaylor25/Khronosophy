
namespace SchedulingDemo.Services;

public static class LoadSampleData
{
    public static User GetUserWithTasks()
    {
        User user = new();

        user.Tasks = new List<Task>();
    }
}
