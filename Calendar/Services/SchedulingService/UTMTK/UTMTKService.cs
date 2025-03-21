using ErrorOr;

using Calendar.Models;
using Calendar.Constants;
using Calendar.Models.Events;

namespace Calendar.Services.SchedulingService.UTMTK;

public class UTMTKService : IUTMTKService
{
    public ErrorOr<Success> ScheduleEvents(KhronosophyUser user)
    {
        ErrorOr<Success> validation = ParameterValidation(user);

        if (validation.IsError)
        {
            return validation.Errors;
        }

        List<TaskboardTask> tasksByImportance =
            user.Taskboard.Tasks
                .OrderByDescending(task => task.Importance)
                .ToList();

        DateOnly dateToSchedule = DateOnly.FromDateTime(DateTime.UtcNow);

        while (CalendarHelpers.ShouldScheduleTasks(tasksByImportance))
        {
            // Console.WriteLine($"scheduling for {dateToSchedule}");

            ScheduleEventsForDay(user, tasksByImportance, dateToSchedule);

            dateToSchedule = dateToSchedule.AddDays(1);
        }

        return new Success();
    }

    /// <summary>
    /// If ParameterValidation(user) does not return Success before this
    /// method is called (or if there is a semantic error in
    /// ParameterValidation(user)), this method may fail an assertion
    /// and crash the program.
    /// </summary>
    private void ScheduleEventsForDay(
        KhronosophyUser user,
        List<TaskboardTask> tasksByImportance,
        DateOnly date
    )
    {
        TimeOnly? nextEventStart = GetNextEventStart(
            date,
            user.DayStart!.Value,
            user.DayEnd!.Value,
            TimeSpan.FromMinutes(user.MinimumEventDurationMinutes!.Value)
        );

        if (nextEventStart == null)
        {
            return;
        }

        TimeSpan timeUntilEndOfDay =
            user.DayEnd!.Value - nextEventStart!.Value;
        double hoursUntilEndOfDay =
            15 * Math.Floor(timeUntilEndOfDay.TotalMinutes / 15) / 60;

        List<EventRequest> tasksForToday =
            SelectTasksForDay(user, tasksByImportance, hoursUntilEndOfDay);

        List<EventRequest> tasksForTodayByIntensity =
            tasksForToday
                .OrderByDescending(task => task.ParentTask?.Intensity ?? 0)
                .ToList();
        // Todo: Restrict adjacent tasks to combined intensity <15.

        PushTimeBlocks(
            user,
            tasksForTodayByIntensity,
            date,
            nextEventStart!.Value
        );
    }

    private List<EventRequest> SelectTasksForDay(
        KhronosophyUser user,
        List<TaskboardTask> tasksByImportance,
        double unscheduledHoursUntilEndOfDay
    )
    {
        List<EventRequest> tasksForDay = [];

        bool eventAddedThisIteration;

        do
        {
            eventAddedThisIteration = false;

            foreach (TaskboardTask taskboardTask in tasksByImportance)
            {
                double averageIntensity = AverageIntensity(tasksForDay);

                // Maximum sum of average and next intensity.
                const double MAGIC_NUMBER = 14;

                bool isIntensityInRange =
                    taskboardTask.Intensity + averageIntensity <=
                        MAGIC_NUMBER;

                bool isTimeRemaining =
                    unscheduledHoursUntilEndOfDay * 60 >
                        user.MinimumEventDurationMinutes;

                bool taskNeedsMoreEvents =
                    CalendarHelpers.ShouldScheduleTask(
                        taskboardTask,
                        tasksForDay
                    );

                if (
                    isIntensityInRange &&
                    isTimeRemaining &&
                    taskNeedsMoreEvents
                )
                {
                    TimeSpan duration = GetDurationForEventRequest(
                        taskboardTask,
                        unscheduledHoursUntilEndOfDay,
                        tasksForDay,
                        user.MinimumEventDurationMinutes!.Value
                    );

                    tasksForDay.Add(
                        new EventRequest(taskboardTask, duration)
                    );

                    unscheduledHoursUntilEndOfDay -=
                        15f * Math.Ceiling(duration.TotalMinutes / 15) / 60;
                    
                    eventAddedThisIteration = true;
                }
            }

            bool isTimeRemainingToSchedule =
                unscheduledHoursUntilEndOfDay * 60 > user.MinimumEventDurationMinutes;
            // Console.WriteLine(unscheduledHoursUntilEndOfDay);

            if (
                !eventAddedThisIteration &&
                isTimeRemainingToSchedule
                // TODO: Check that there is time to be scheduled in taskboard.
            )
            {
                tasksForDay.Add(UTMTKConstants.EVENT_REQUEST_BREAK);
                unscheduledHoursUntilEndOfDay -=
                    UTMTKConstants.BREAK_DURATION;
                eventAddedThisIteration = true;
            }
        }
        while(eventAddedThisIteration);

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

    private void PushTimeBlocks(
        KhronosophyUser user,
        List<EventRequest> eventRequests,
        DateOnly date,
        TimeOnly nextEventStart
    )
    {
        // Handle explicit time breaks.

        foreach (EventRequest eventRequest in eventRequests)
        {
            if (
                eventRequest.ParentTask is TaskboardTask parentTask &&
                parentTask.Intensity is double intensity &&
                nextEventStart.Add(eventRequest.Duration) <= user.DayEnd
            )
            {
                DateTime startDateTime = new(date, nextEventStart);

                ScheduledEvent scheduledEvent = new(
                    parentTask.Name,
                    startDateTime,
                    startDateTime + eventRequest.Duration,
                    parentTask.Id
                );

                parentTask.Events.Add(scheduledEvent);
                user.EventCalendar.Events.Add(scheduledEvent);

                double breakDurationHours =
                    eventRequest.Duration.TotalHours * intensity / 20;
                double breakDurationHoursRounded =
                    15 * Math.Ceiling(breakDurationHours * 60 / 15) / 60;
                TimeSpan breakDuration =
                    TimeSpan.FromHours(breakDurationHoursRounded);

                nextEventStart =
                    TimeOnly.FromDateTime(scheduledEvent.EndDateTime)
                        .Add(breakDuration, out int _wrappedDays);

                if (_wrappedDays != 0)
                {
                    // Console.WriteLine(
                    //     "No further events can be scheduled today."
                    // );
                    return;
                }
            }
        }
    }

    private TimeSpan GetDurationForEventRequest(
        TaskboardTask taskboardTask,
        double unscheduledHoursUntilEndOfDay,
        List<EventRequest> tasksForDay,
        int minimumEventDurationMinutes
    )
    {
        if (taskboardTask.Intensity is double intensity)
        {
            // Linear interpolation of:
            // intensity  0 --> 4 hours
            // intensity 10 --> 1 hour
            double maxTaskTimeHours = 4 - (3 * intensity / 10);

            // Round up to the next 15 minutes.
            double maxTaskTimeHoursQuarterHourIncrement =
                Math.Ceiling(4 * maxTaskTimeHours) / 4;

            double remainingTaskDurationToScheduleMinutes =
                taskboardTask.TimeToBeScheduled().TotalMinutes -
                CalendarHelpers.TimeRequestedMinutes(
                    tasksForDay,
                    taskboardTask
                );


            double durationHours = Math.Min(
                Math.Min(
                    maxTaskTimeHoursQuarterHourIncrement,
                    unscheduledHoursUntilEndOfDay
                ),
                Math.Max(
                    remainingTaskDurationToScheduleMinutes / 60d,
                    minimumEventDurationMinutes / 60d
                )
            );

            return TimeSpan.FromHours(durationHours);
        }

        // TODO: handle better?
        return TimeSpan.FromHours(0);
    }

    private TimeOnly? GetNextEventStart(
        DateOnly date,
        TimeOnly dayStart,
        TimeOnly dayEnd,
        TimeSpan minimumEventDuration
        // previousTask
    )
    {
        // We want to restrict this function to a specified date.
        // This variable keeps track of if we've crossed into
        // the next date.
        int _wrappedDays;

        TimeOnly nextEventStart = dayStart;

        DateTime now = DateTime.UtcNow;
        DateOnly dateNow = DateOnly.FromDateTime(now);
        TimeOnly timeNow = TimeOnly.FromDateTime(now);

        // If we're scheduling for today and we're already
        // past dayStart, we'll be scheduling for the past.
        // Correct this by shifting the nextEventStart to
        // the next whole hour.
        if (date == dateNow && timeNow > nextEventStart)
        {
            nextEventStart = new TimeOnly(
                timeNow.Hour,
                0,
                0
            )
                .AddHours(1, out _wrappedDays);

            #region Error Handling
            if (_wrappedDays != 0)
            {
                return null;
            }
            #endregion
        }

        // When scheduling more than one event, all will be given
        // the same nextEventStart unless we offset them from the
        // the previous.
        if (false)
        {

        }

        // If there is not enough time in the day to satisfy the
        // minimum event time, return null.
        if (
            nextEventStart.Add(
                minimumEventDuration,
                out _wrappedDays
            ) > dayEnd ||
            _wrappedDays != 0
        )
        {
            return null;
        }

        return nextEventStart;
    }

    private ErrorOr<Success> ParameterValidation(
        KhronosophyUser user
    )
    {
        if (
            user.DailyIntensityCapacity == null ||
            user.DayStart == null ||
            user.DayEnd == null ||
            user.MinimumEventDurationMinutes == null
        )
        {
            return Error.Validation("User settings unset");
        }

        if (
            user.DailyIntensityCapacity <= 0 ||
            user.DailyIntensityCapacity >
                UTMTKConstants.MAX_DAILY_INTENSITY_CAPACITY ||
            // day start divisible by 15 mins
            // day end divisible by 15 mins
            user.DayStart >= user.DayEnd ||
            user.MinimumEventDurationMinutes <= 0 ||
            user.MinimumEventDurationMinutes >
                UTMTKConstants.MAX_MINIMUM_EVENT_DURATION_MINUTES ||
            user.MinimumEventDurationMinutes % 15 != 0
        )
        {
            return Error.Validation("User settings invalid");
        }

        foreach (TaskboardTask taskboardTask in user.Taskboard.Tasks)
        {
            if (
                taskboardTask.Importance == null ||
                taskboardTask.Intensity == null
            )
            {
                return Error.Validation("Task properties unset");
            }

            if (
                taskboardTask.Importance <
                    UTMTKConstants.MIN_TASK_IMPORTANCE ||
                taskboardTask.Importance >
                    UTMTKConstants.MAX_TASK_IMPORTANCE ||
                taskboardTask.Intensity <
                    UTMTKConstants.MIN_TASK_INTENSITY ||
                taskboardTask.Intensity >
                    UTMTKConstants.MAX_TASK_INTENSITY
            )
            {
                return Error.Validation("Task properties invalid");
            }
        }

        return new Success();
    }
}
