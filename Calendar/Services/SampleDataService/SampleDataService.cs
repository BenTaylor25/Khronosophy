
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
