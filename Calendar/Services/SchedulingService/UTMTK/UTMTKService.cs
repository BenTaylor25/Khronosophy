using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.SchedulingService.UTMTK;

public class UTMTKService : IUTMTKService
{
    public ErrorOr<Success> ScheduleEvents(KhronosophyUser user)
    {

        List<TaskboardTask> tasksByImportance =
            user.Taskboard.Tasks
                .OrderByDescending(task => task.Importance)
                .ToList();

        DateOnly dateToSchedule = DateOnly.FromDateTime(DateTime.UtcNow);

        while (CalendarHelpers.ShouldScheduleTasks(tasksByImportance))
        {

        }

        return new Success();
    }
}
