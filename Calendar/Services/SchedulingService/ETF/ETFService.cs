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

        // tasks by importance

        // time in working day

        // schedule for day

        return new Success();
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
