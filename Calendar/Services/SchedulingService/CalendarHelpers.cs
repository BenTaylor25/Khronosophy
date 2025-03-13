
using Calendar.Models;
using Calendar.Models.Events;

namespace Calendar.Services.SchedulingService;

public static class CalendarHelpers
{
    /// <summary>
    /// Using the expected duration of each task and the total duration of
    /// their respective child events, determine whether further events
    /// need to be created.
    /// </summary>
    public static bool ShouldScheduleTasks(List<TaskboardTask> tasks)
    {
        foreach (TaskboardTask taskboardTask in tasks)
        {
            double expectedTimeMinutes =
                taskboardTask.ExpectedDuration.TotalMinutes;

            double scheduledTimeMinutes = 0;

            foreach (ScheduledEvent scheduledEvent in taskboardTask.Events)
            {
                scheduledTimeMinutes +=
                    scheduledEvent.Duration.TotalMinutes;
            }

            if (scheduledTimeMinutes < expectedTimeMinutes)
            {
                return true;
            }
        }

        return false;
    }
}
