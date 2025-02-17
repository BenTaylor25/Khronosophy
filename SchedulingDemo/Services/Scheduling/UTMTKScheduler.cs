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
            user.Taskboard.Tasks
                .OrderByDescending(task => task.Importance)
                .ToList();

        DateTime nextEventStart = new DateTime(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            DateTime.UtcNow.Day,
            DateTime.UtcNow.Hour,
            0,
            0
        )
            .AddHours(1);

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
                .AddDays(1);
        }

        TimeSpan timeUntilEndOfDay =
            DAY_END - TimeOnly.FromDateTime(nextEventStart);
        double hoursUntilEndOfDay =
            Helpers.TimeSpanDownToDecimalHours(timeUntilEndOfDay);

        bool isTimeLeftInDay =
            timeUntilEndOfDay >= MINIMIUM_EVENT_LENGTH;

        List<EventRequest> tasksForToday =
            SelectTasksForDay(user, tasksByImportance, hoursUntilEndOfDay);

        // Console.WriteLine("tasks for today");
        // Console.WriteLine(tasksForToday.Count);
        // foreach (EventRequest eventRequest in tasksForToday) {
        //     Console.WriteLine(eventRequest.ParentTask?.Name);
        // }

        List<EventRequest> tasksForTodayByIntensity =
            tasksForToday
                .OrderByDescending(task => task.ParentTask?.Intensity ?? 0)
                .ToList();
        // TODO: Restrict adjacent tasks to combined intensity < 15.

        // Console.WriteLine("tasks for today");
        // Console.WriteLine(tasksForToday.Count);
        // foreach (EventRequest eventRequest in tasksForTodayByIntensity) {
        //     Console.WriteLine(eventRequest.ParentTask?.Name);
        // }

        PushTimeBlocks(user, tasksForTodayByIntensity, nextEventStart);
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
        double unscheduledHoursUntilEndOfDay
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

                if (
                    task.Intensity + averageIntensity <= MAGIC_NUMBER &&
                    unscheduledHoursUntilEndOfDay > 1
                )
                {
                    // TODO: Check that task needs to be scheduled more time.

                    TimeSpan duration =
                        GetDurationForEventRequest(
                            task,
                            unscheduledHoursUntilEndOfDay
                        );

                    tasksForDay.Add(
                        new EventRequest(task, duration)
                    );

                    unscheduledHoursUntilEndOfDay -=
                        Helpers.TimeSpanUpToDecimalHours(duration);

                    eventAddedThisIteration = true;
                }
            }

            const int MAX_UNACCOUNTED_TIME = 1;

            if (
                !eventAddedThisIteration &&
                unscheduledHoursUntilEndOfDay > MAX_UNACCOUNTED_TIME
                // TODO: Check that their is time to be scheduled in taskboard.
            )
            {
                tasksForDay.Add(Constants.EVENT_REQUEST_BREAK);
                unscheduledHoursUntilEndOfDay -= 1;
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

        double totalIntensity = 0;

        foreach (EventRequest eventRequest in eventRequests)
        {
            totalIntensity += eventRequest.ParentTask?.Intensity ?? 0;
        }

        double averageIntensity = totalIntensity / eventRequests.Count;
        return averageIntensity;
    }

    public static void PushTimeBlocks(
        User user,
        List<EventRequest> eventRequests,
        DateTime nextEventStart
    )
    {
        // Handle explicit time breaks.

        foreach (EventRequest eventRequest in eventRequests)
        {
            if (
                eventRequest.ParentTask is TaskboardTask parentTask &&
                parentTask.Intensity is double intensity
            )
            {
                ScheduledEvent scheduledEvent = new(
                    parentTask.Name,
                    nextEventStart,
                    nextEventStart + eventRequest.Duration,
                    parentTask
                );

                parentTask.Events.Add(scheduledEvent);
                user.Calendar.Events.Add(scheduledEvent);

                TimeSpan breakDuration = TimeSpan.FromHours(
                    eventRequest.Duration.TotalHours *
                    intensity / 20
                );

                nextEventStart =
                    scheduledEvent.EndDateTime + breakDuration;
            }
        }
    }

    private static TimeSpan GetDurationForEventRequest(
        TaskboardTask task,
        double unscheduledHoursUntilEndOfDay
    )
    {
        if (task.Intensity is double intensity)
        {
            // Linear interpolation of:
            // intensity  0 --> 4 hours
            // intensity 10 --> 1 hour
            double maxTaskTimeHours = 4 - (3 * intensity / 10);

            // Round up to the next 15 minutes.
            double maxTaskTimeHoursQuarterHourIncrement =
                Math.Ceiling(4 * maxTaskTimeHours) / 4;

            // double remainingTaskDurationToScheduleHours = something on task

            double durationHours = Math.Min(
                maxTaskTimeHoursQuarterHourIncrement,
                unscheduledHoursUntilEndOfDay
                // task duration to schedule
            );

            return TimeSpan.FromHours(durationHours);
        }

        // TODO: handle better?
        return TimeSpan.FromHours(0);
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

