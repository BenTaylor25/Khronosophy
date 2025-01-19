using SchedulingDemo.Models;

namespace SchedulingDemo.Services;

public class DumbScheduler : IScheduler
{
    // 8AM - 8PM
    private readonly TimeOnly DAY_START = new(8,0,0);
    private readonly TimeOnly DAY_END = new(20,0,0);

    private readonly TimeSpan MINIMIUM_EVENT_LENGTH = new(1, 0, 0);

    public void ScheduleUsersTasks(User user)
    {
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
            nextEventStart = new(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day + 1,
                DAY_START.Hour,
                DAY_START.Minute,
                DAY_START.Second
            );
        }

        // Schedule
        foreach (TaskboardTask task in user.Taskboard.Tasks)
        {
            TimeSpan timeAlreadyScheduled =
                task.Events.Aggregate(
                    TimeSpan.Zero,
                    (sum, calendarEvent) => sum + calendarEvent.Duration
                );

            // ExpectedDuration * 1.5?
            TimeSpan timeToSchedule =
                task.ExpectedDuration - timeAlreadyScheduled;

            while (timeToSchedule > TimeSpan.Zero)
            {
                TimeSpan timeUntilEndOfDay =
                    DAY_END - TimeOnly.FromDateTime(nextEventStart);

                bool isTimeLeftInDay =
                    timeUntilEndOfDay >= MINIMIUM_EVENT_LENGTH;

                if (!isTimeLeftInDay)
                {
                    nextEventStart =
                        new DateTime(
                            nextEventStart.Year,
                            nextEventStart.Month,
                            nextEventStart.Day + 1,
                            DAY_START.Hour,
                            DAY_START.Minute,
                            DAY_START.Second
                        );

                    timeUntilEndOfDay =
                        DAY_END - TimeOnly.FromDateTime(nextEventStart);
                }

                // Select minimum of time to schedule and time left in day.
                TimeSpan eventDuration =
                    timeToSchedule > timeUntilEndOfDay ?
                    timeUntilEndOfDay : timeToSchedule;

                ScheduledEvent scheduledEvent = new(
                    task.Name,
                    nextEventStart,
                    nextEventStart + eventDuration,
                    task
                );

                task.Events.Add(scheduledEvent);
                user.Calendar.Events.Add(scheduledEvent);

                timeToSchedule -= eventDuration;
                nextEventStart = scheduledEvent.EndDateTime;
            }
        }
    }
}

