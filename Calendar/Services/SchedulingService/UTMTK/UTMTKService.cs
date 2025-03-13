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
            return validation.Errors;
        }

        List<TaskboardTask> tasksByImportance =
            user.Taskboard.Tasks
                .OrderByDescending(task => task.Importance)
                .ToList();

        DateOnly dateToSchedule = DateOnly.FromDateTime(DateTime.UtcNow);

        while (CalendarHelpers.ShouldScheduleTasks(tasksByImportance))
        {


            dateToSchedule.AddDays(1);
        }

        return new Success();
    }

    private static ErrorOr<Success> ParameterValidation(
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
