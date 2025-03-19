using ErrorOr;

using Calendar.Models;
using Calendar.Constants;

namespace Calendar.Services.SchedulingService.UTMTK;

public class UTMTKService : IUTMTKService
{
    public ErrorOr<Success> ScheduleEvents(KhronosophyUser user)
    {
        ErrorOr<Success> validation = ParameterValidation(user);

        if (validation.IsError)
        {
            return validation;
        }

        List<TaskboardTask> tasksByImportance =
            user.Taskboard.Tasks
                .OrderByDescending(task => task.Importance)
                .ToList();

        DateOnly dateToSchedule = DateOnly.FromDateTime(DateTime.UtcNow);

        while (CalendarHelpers.ShouldScheduleTasks(tasksByImportance))
        {
            ScheduleEventsForDay(user, tasksByImportance, dateToSchedule);

            dateToSchedule.AddDays(1);
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

        // TimeOnly nextEventStart = user.DayStart!.Value;
        TimeOnly nextEventStart = GetNextEventStart(
            date,
            user.DayStart,
            user.DayEnd,
            // user.MinimumEventDurationMinutes
        );


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
            user.DayStart < user.DayEnd ||
            user.MinimumEventDurationMinutes < 0 ||
            user.MinimumEventDurationMinutes >
                UTMTKConstants.MAX_MINIMUM_EVENT_DURATION_MINUTES ||
            user.MinimumEventDurationMinutes % 15 == 0
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
                    UTMTKConstants.MIN_TASK_IMPORTANCE ||
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
