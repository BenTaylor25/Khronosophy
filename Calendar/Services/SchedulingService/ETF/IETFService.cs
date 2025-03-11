using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.SchedulingService.ETF;

public interface IETFService
{
    ErrorOr<Success> ScheduleEvents(KhronosophyUser user);
}
