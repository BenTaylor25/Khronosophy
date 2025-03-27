using ErrorOr;

using Calendar.Models;
using Calendar.Models.Events;
using Calendar.Constants;

namespace Calendar.Services.SchedulingService.ETF;

public class ETFService : IETFService
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

        TimeOnly workingDayEnd = CalculateWorkingDayEnd(
            user.DayStart!.Value,
            user.DayEnd!.Value
        );

        DateOnly dateToSchedule = DateOnly.FromDateTime(DateTime.UtcNow);

        while (CalendarHelpers.ShouldScheduleTasks(tasksByImportance))
        {
            ScheduleEventsForDay(
                user,
                tasksByImportance,
                workingDayEnd,
                dateToSchedule
            );

            dateToSchedule = dateToSchedule.AddDays(1);
        }

        return new Success();
    }

    private void ScheduleEventsForDay(
        KhronosophyUser user,
        List<TaskboardTask> tasksByImportance,
        TimeOnly workingDayEnd,
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

        TimeSpan timeUntilEndOfWorkingDay =
            workingDayEnd - nextEventStart!.Value;

        foreach (TaskboardTask taskboardTask in tasksByImportance)
        {
            if (
                timeUntilEndOfWorkingDay.TotalMinutes >=
                user.MinimumEventDurationMinutes
            )
            {
                TimeSpan taskRemainingDuration =
                    RemainingDuration(taskboardTask);

                // Min(taskRemainingDuration, timeUntilEndOfWorkingDay);
                TimeSpan eventDuration =
                    taskRemainingDuration > timeUntilEndOfWorkingDay ?
                    timeUntilEndOfWorkingDay : taskRemainingDuration;

                DateTime startDateTime = new(date, nextEventStart!.Value);

                ScheduledEvent scheduledEvent = new(
                    taskboardTask.Name,
                    startDateTime,
                    startDateTime + eventDuration,
                    taskboardTask.Id
                );

                taskboardTask.Events.Add(scheduledEvent);
                user.EventCalendar.Events.Add(scheduledEvent);

                nextEventStart =
                    TimeOnly.FromDateTime(scheduledEvent.EndDateTime)
                        .Add(
                            ETFConstants.BREAK_DURATION,
                            out int _wrappedDays
                        );

                timeUntilEndOfWorkingDay -=
                    eventDuration + ETFConstants.BREAK_DURATION;
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

    private TimeOnly CalculateWorkingDayEnd(
        TimeOnly dayStart,
        TimeOnly dayEnd
    )
    {
        TimeSpan dayDuration = dayEnd - dayStart;
        double dayDurationMinutes = dayDuration.TotalMinutes;

        double workingDayDurationMinutes =
            dayDurationMinutes * ETFConstants.WORKING_DAY_RATIO;
        double workingDayDurationMinutesRounded =
            15 * Math.Floor(workingDayDurationMinutes / 15);
        TimeSpan workingDayDuration =
            TimeSpan.FromMinutes(workingDayDurationMinutesRounded);

        TimeOnly workingDayEnd = dayStart.Add(
            workingDayDuration,
            out int _wrappedDays
        );

        #region Error Handling
        if (_wrappedDays != 0)
        {
            // Something has gone horribly wrong.
            throw new InvalidOperationException(
                "Working Day End calculation overflowed"
            );
        }
        #endregion

        return workingDayEnd;
    }

    private ErrorOr<Success> ParameterValidation(KhronosophyUser user)
    {
        if (
            user.DayStart == null ||
            user.DayEnd == null ||
            user.MinimumEventDurationMinutes == null
        )
        {
            return Error.Validation("User settings unset");
        }

        if (
            // day start divisible by 15 mins
            // day end divisible by 15 mins
            user.DayStart >= user.DayEnd ||
            user.MinimumEventDurationMinutes <= 0 ||
            user.MinimumEventDurationMinutes >
                ETFConstants.MAX_MINIMUM_EVENT_DURATION_MINUTES ||
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
                    ETFConstants.MIN_TASK_IMPORTANCE ||
                taskboardTask.Importance >
                    ETFConstants.MAX_TASK_IMPORTANCE ||
                taskboardTask.Intensity <
                    ETFConstants.MIN_TASK_INTENSITY ||
                taskboardTask.Intensity >
                    ETFConstants.MAX_TASK_INTENSITY
            )
            {
                return Error.Validation("Task properties invalid");
            }
        }

        return new Success();
    }

    private TimeOnly? GetNextEventStart(
        DateOnly date,
        TimeOnly dayStart,
        TimeOnly dayEnd,
        TimeSpan minimumEventDuration
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
}
