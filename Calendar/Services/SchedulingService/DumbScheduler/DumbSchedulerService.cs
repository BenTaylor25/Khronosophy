using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.SchedulingService.DumbScheduler;

public class DumbSchedulerService : IDumbSchedulerService
{
    public ErrorOr<Success> ScheduleEvents(KhronosophyUser user)
    {

        return new Success();
    }
}
