using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.SchedulingService.DumbScheduler;

public interface IDumbSchedulerService
{
    ErrorOr<Success> ScheduleEvents(KhronosophyUser user);
}
