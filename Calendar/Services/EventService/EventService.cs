
using Calendar.Models;
using Calendar.Models.Events;
using ErrorOr;

namespace Calendar.Services.EventService;

public class EventService : IEventService
{
    public ErrorOr<List<IEvent>> GetUserEvents(KhronosophyUser user)
    {
        return user.EventCalendar.Events;
    }

    public ErrorOr<Success> AddEventToUser(
        KhronosophyUser user,
        StaticEvent staticEvent
    )
    {
        user.EventCalendar.Events.Add(staticEvent);
        return new Success();
    }
}
