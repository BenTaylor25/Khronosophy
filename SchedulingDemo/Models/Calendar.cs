
namespace KhronosScheduling.Models;

public class Calendar
{
    public List<IEvent> Events { get; set; }

    public Calendar()
    {
        Events = [];
    }
}

