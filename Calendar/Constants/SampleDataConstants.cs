using ErrorOr;

using Calendar.Models;

namespace Calendar.Constants;

public static class SampleDataConstants
{
    private static TaskboardTask CreateTask(
        string name,
        TimeSpan expectedDuration,
        int importance,
        int intensity
    )
    {
        ErrorOr<TaskboardTask> taskOrError = TaskboardTask.Create(
            name, expectedDuration, importance, intensity
        );

        if (taskOrError.IsError)
        {
            throw new Exception("Could not create sample data");
        }
        return taskOrError.Value;
    }

    public static readonly TaskboardTask[] TASKS_WITH_INTENSITIES = [
        CreateTask("test 1", TimeSpan.FromHours(3), 3, 8),
        CreateTask("test 2", TimeSpan.FromHours(4), 2, 7),
        CreateTask("test 3", TimeSpan.FromHours(2), 1, 10),
        CreateTask("test 4", TimeSpan.FromHours(6), 5, 2),
        CreateTask("test 5", TimeSpan.FromHours(3), 4, 9),
        CreateTask("test 6", TimeSpan.FromHours(1), 6, 10),
        CreateTask("test 7", TimeSpan.FromHours(3), 7, 7),
        CreateTask("test 8", TimeSpan.FromHours(2), 10, 9),
        CreateTask("test 9", TimeSpan.FromHours(2), 9, 8),
    ];
}
