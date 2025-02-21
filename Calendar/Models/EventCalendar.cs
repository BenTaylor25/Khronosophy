using Calendar.Models.Events;

namespace Calendar.Models;

public class EventCalendar
{
    public List<IEvent> Events { get; set; } = [];
}
