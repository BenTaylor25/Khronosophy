
using Calendar.Models;
using ErrorOr;

namespace Calendar.Services.SchedulingService.UTMTK;

public class UTMTKService : IUTMTKService
{
    public ErrorOr<Success> ScheduleEvents(KhronosophyUser user)
    {

        return new Success();
    }
}
