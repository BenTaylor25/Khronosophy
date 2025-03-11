using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.SchedulingService.UTMTK;

public interface IUTMTKService
{
    ErrorOr<Success> ScheduleEvents(KhronosophyUser user);
}
