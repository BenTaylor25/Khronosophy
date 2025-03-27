using ErrorOr;

using Calendar.Models;
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
            ScheduleEventsForDay(user, tasksByImportance, dateToSchedule);

            dateToSchedule = dateToSchedule.AddDays(1);
        }

        return new Success();
    }

    private void ScheduleEventsForDay(
        KhronosophyUser user,
        List<TaskboardTask> tasksByImportance,
        DateOnly date
    )
    {

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
}
