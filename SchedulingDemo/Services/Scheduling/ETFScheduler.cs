
using SchedulingDemo.Models;

namespace SchedulingDemo.Services.Scheduling;

public class ETFScheduler : IScheduler
{
    // 8AM - 8PM
    private readonly TimeOnly DAY_START = new(8,0,0);
    private readonly TimeOnly DAY_END = new(20,0,0);

    private readonly double WORK_LEISURE_RATIO = 0.667;

    private readonly TimeSpan MINIMIUM_EVENT_LENGTH = new(1, 0, 0);

    private readonly TimeSpan BREAK_DURATION = new(0, 15, 0);

    /// <summary>
    /// ETF Philosophy:
    /// Work hard early, enjoy the evening. <br />
    /// <br />
    /// ETF Method:
    /// Split the day into 3rds.
    /// The first 2/3rds of the day is dedicated to working in order of
    //. Importance. Q: WHAT DOES ETF SAY ABOUT BREAKS?
    /// The final 1/3rd cannot schedule events with an Intensity higher
    /// than 1. It's implied to be left empty (for the user to fill with
    /// leisurely static events manually).
    /// </summary>
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

        TimeSpan dayDuration = DAY_END - DAY_START;

        // Work Duration rounded to 15 minutes.
        TimeSpan workDuration = dayDuration * WORK_LEISURE_RATIO;
        workDuration = TimeSpan.FromMinutes(
            15 * Math.Floor(workDuration.TotalMinutes / 15)
        );

        TimeOnly workCutoffTime = DAY_START.Add(workDuration);

        DateTime nextEventStart = new DateTime(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            DateTime.UtcNow.Day,
            DAY_START.Hour,
            DAY_START.Minute,
            DAY_START.Second
        )
            .AddDays(1);

        TimeSpan timeUntilEndOfWorkingDay =
            workCutoffTime - TimeOnly.FromDateTime(nextEventStart);

        foreach (TaskboardTask taskboardTask in tasksByImportance)
        {
            if (timeUntilEndOfWorkingDay >= MINIMIUM_EVENT_LENGTH)
            {
                TimeSpan taskRemainingDuration =
                    RemainingDuration(taskboardTask);

                // Min(taskRemainingDuration, timeUntilEndOfWorkingDay);
                TimeSpan eventDuration =
                    taskRemainingDuration > timeUntilEndOfWorkingDay ?
                    timeUntilEndOfWorkingDay : taskRemainingDuration;

                ScheduledEvent scheduledEvent = new(
                    taskboardTask.Name,
                    nextEventStart,
                    nextEventStart + eventDuration,
                    taskboardTask
                );

                taskboardTask.Events.Add(scheduledEvent);
                user.Calendar.Events.Add(scheduledEvent);

                nextEventStart =
                    scheduledEvent.EndDateTime + BREAK_DURATION;
                timeUntilEndOfWorkingDay -= eventDuration + BREAK_DURATION;
            }
        }
    }

    private TimeSpan RemainingDuration(TaskboardTask task)
    {
        TimeSpan remainingDuration = task.ExpectedDuration;

        foreach (ScheduledEvent scheduledEvent in task.Events)
        {
            remainingDuration -= scheduledEvent.Duration;
        }

        if (remainingDuration < TimeSpan.Zero)
        {
            remainingDuration = TimeSpan.Zero;
        }

        return remainingDuration;
    }

    private bool AreParametersValid(User user)
    {
        if (user.DailyIntensityCapacity == null)
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
