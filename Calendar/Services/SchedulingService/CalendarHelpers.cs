
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
            if (ShouldScheduleTask(taskboardTask))
            {
                return true;
            }
        }

        return false;
    }

    public static bool ShouldScheduleTask(
        TaskboardTask taskboardTask,
        List<EventRequest>? eventRequests = null
    )
    {
        double timeToScheduleMinutes =
            taskboardTask.TimeToBeScheduled().TotalMinutes;

        if (eventRequests != null)
        {
            timeToScheduleMinutes -=
                TimeRequestedMinutes(eventRequests, taskboardTask);
        }

        return timeToScheduleMinutes > 0;
    }

    /// <summary>
    /// Return the total time requested either by a particular task or
    /// overall (if taskboardTask is not specified).
    /// </summary>
    public static double TimeRequestedMinutes(
        List<EventRequest> eventRequests,
        TaskboardTask? taskboardTask = null
    )
    {
        double timeRequestedMinutes = 0;

        foreach (EventRequest eventRequest in eventRequests)
        {
            bool thisTaskIsSpecified =
                eventRequest.ParentTask == taskboardTask;

            bool shouldCountAllTasks = taskboardTask == null;

            if (thisTaskIsSpecified || shouldCountAllTasks)
            {
                timeRequestedMinutes += eventRequest.Duration.TotalMinutes;
            }
        }

        return timeRequestedMinutes;
    }
}
