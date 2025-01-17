
namespace SchedulingDemo.Models;

public class StaticEvent(
    string name,
    DateTime startDateTime,
    DateTime endDateTime
) : IEvent
{
    public string Name { get; set;} = name;
    public DateTime StartDateTime { get; set;} = startDateTime;
    public DateTime EndDateTime { get; set; } = endDateTime;

    public TimeSpan Duration
    {
        get => EndDateTime - StartDateTime;
        set => EndDateTime = StartDateTime + value;
    }
}

