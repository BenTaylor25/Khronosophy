
using Calendar.Constants;
using Calendar.Models;
using ErrorOr;

namespace Calendar.Services.SampleDataService;

public class SampleDataService : ISampleDataService
{
    public ErrorOr<Success> LoadTasksAndIntensities(
        KhronosophyUser user
    )
    {
        user.DayStart = SampleDataConstants.DAY_START;
        user.DayEnd = SampleDataConstants.DAY_END;
        user.DailyIntensityCapacity =
            SampleDataConstants.MAX_DAILY_INTENSITY_CAPACITY;
        user.MinimumEventDurationMinutes =
            SampleDataConstants.MINIMUM_EVENT_DURATION_MINUTES;

        foreach (
            TaskboardTask taskboardTask in
            SampleDataConstants.TASKS_WITH_INTENSITIES
        )
        {
            user.Taskboard.Tasks.Add(taskboardTask);
        }

        return new Success();
    }
}
