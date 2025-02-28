using ErrorOr;

using Calendar.Models;
using Calendar.Models.Events;

namespace Calendar.Services.EventService;

public interface IEventService
{
    ErrorOr<List<IEvent>> GetUserEvents(KhronosophyUser user);
    ErrorOr<Success> AddEventToUser(
        KhronosophyUser user,
        StaticEvent staticEvent
    );
}
