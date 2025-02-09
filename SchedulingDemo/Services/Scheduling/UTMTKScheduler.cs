using SchedulingDemo.Models;
using SchedulingDemo.Models.Enums;
using SchedulingDemo.Models.Settings;

namespace SchedulingDemo.Services.Scheduling;

public class UTMTKScheduler(UTMTKSettings settings) : IScheduler
{
    // 8AM - 8PM
    private readonly TimeOnly DAY_START = new(8,0,0);
    private readonly TimeOnly DAY_END = new(20,0,0);

    private readonly TimeSpan MINIMIUM_EVENT_LENGTH = new(1, 0, 0);

    private UTMTKSettings Settings { get; set; } = settings;

    public void ScheduleUsersTasks(User user)
    {
        if (!AreParametersValid(user))
        {
            Console.WriteLine("User Invalid");
            return;   // Error message.
        }

        List<TaskboardTask> tasksByImportance =
            user.Taskboard.Tasks.OrderBy(task => task.Importance).ToList();

        DateTime nextEventStart = new(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            DateTime.UtcNow.Day,
            DateTime.UtcNow.Hour + 1,
            0,
            0
        );

        if (TimeOnly.FromDateTime(nextEventStart) >= DAY_END)
        {
            nextEventStart = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                DAY_START.Hour,
                DAY_START.Minute,
                DAY_START.Second
            )
                .AddHours(1);
        }

        TimeSpan timeUntilEndOfDay =
            DAY_END - TimeOnly.FromDateTime(nextEventStart);

        bool isTimeLeftInDay =
            timeUntilEndOfDay >= MINIMIUM_EVENT_LENGTH;

        List<TaskboardTask> tasksForToday =
            SelectTasksForToday(user, tasksByImportance);

        Console.WriteLine("tasks for today");
        foreach (TaskboardTask task in tasksForToday) {
            Console.WriteLine(task.Name);
        }
    }

    private List<TaskboardTask> SelectTasksForToday(
        User user,
        List<TaskboardTask> tasksByImportance
    )
    {
        List<TaskboardTask> tasksForToday = [];

        int runningCapacity = 0;

        foreach (TaskboardTask task in tasksByImportance)
        {
            if (task.Intensity is Intensity intensity)
            {
                int? intensityValue =
                    IntensityHelper.IntensityValue(intensity, Settings);

                bool hasCapacityForTask =
                    runningCapacity + intensityValue <=
                    user.DailyIntensityCapacity;

                if (hasCapacityForTask)
                {
                    // intensityValue is not null here.
                    runningCapacity += intensityValue ?? 0;
                    tasksForToday.Add(task);
                }
            }
        }

        return tasksForToday;
    }

    private bool AreParametersValid(User user)
    {
        if (user.DailyIntensityCapacity == null)
        {
            return false;
        }

        bool highIntensityGreaterThanDailyCapacity =
            Settings.HighIntensityValue > user.DailyIntensityCapacity;

        bool intensityValuesNotInOrder =
            Settings.HighIntensityValue < Settings.MediumIntensityValue ||
            Settings.MediumIntensityValue < Settings.LowIntensityValue;

        if (
            highIntensityGreaterThanDailyCapacity ||
            intensityValuesNotInOrder
        )
        {
            return false;
        }

        foreach (TaskboardTask task in user.Taskboard.Tasks)
        {
            if (
                task.Importance == null ||
                task.Intensity == null
            )
            {
                return false;
            }
        }

        return true;
    }
}

