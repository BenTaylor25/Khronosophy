using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.SchedulingService.ETF;

public class ETFService : IETFService
{
    public ErrorOr<Success> ScheduleEvents(KhronosophyUser user)
    {

        return new Success();
    }
}
