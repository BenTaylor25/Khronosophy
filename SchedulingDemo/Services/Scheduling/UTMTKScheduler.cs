using SchedulingDemo.Models;
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
        double hoursUntilEndOfDay =
            Helpers.TimeSpanDownToDecimalHours(timeUntilEndOfDay);

        bool isTimeLeftInDay =
            timeUntilEndOfDay >= MINIMIUM_EVENT_LENGTH;

        List<EventRequest> tasksForToday =
            SelectTasksForDay(user, tasksByImportance, hoursUntilEndOfDay);

        Console.WriteLine("tasks for today");
        foreach (EventRequest eventRequest in tasksForToday) {
            Console.WriteLine(eventRequest.ParentTask?.Name);
        }
    }

    /// <summary>
    /// Priority 1: keep intensity balance in check.
    /// <br />
    /// Priority 2: if there are insufficient low-intensity tasks,
    /// introduce extra time breaks.
    /// <br />
    /// Priority 3: select the most important task.
    /// </summary>
    private List<EventRequest> SelectTasksForDay(
        User user,
        List<TaskboardTask> tasksByImportance,
        double hoursUntilEndOfDay
    )
    {
        List<EventRequest> tasksForDay = [];

        bool eventAddedThisIteration;

        do
        {
            eventAddedThisIteration = false;

            foreach (TaskboardTask task in tasksByImportance)
            {
                double averageIntensity = AverageIntensity(tasksForDay);

                // Maximum sum of average and next intensity.
                const double MAGIC_NUMBER = 14;

                if (task.Intensity + averageIntensity <= MAGIC_NUMBER)
                {
                    // TODO: Check that task needs to be scheduled more time.

                    TimeSpan duration =
                        TimeSpan.FromHours(hoursUntilEndOfDay);

                    tasksForDay.Add(
                        new EventRequest(task, duration)
                    );

                    eventAddedThisIteration = true;
                }
            }

            const int MAX_UNACCOUNTED_TIME = 1;

            if (
                !eventAddedThisIteration &&
                hoursUntilEndOfDay > MAX_UNACCOUNTED_TIME
                // TODO: Check that their is time to be scheduled in taskboard.
            )
            {
                tasksForDay.Add(Constants.EVENT_REQUEST_BREAK);
                eventAddedThisIteration = true;
            }
        }
        while (eventAddedThisIteration);

        return tasksForDay;
    }

    private static double AverageIntensity(
        List<EventRequest> eventRequests
    )
    {
        if (eventRequests.Count == 0)
        {
            return 0;
        }

        int totalIntensity = 0;

        foreach (EventRequest eventRequest in eventRequests)
        {
            // totalIntensity += eventRequest.Intensity;
        }

        double averageIntensity = totalIntensity / eventRequests.Count;
        return averageIntensity;
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

