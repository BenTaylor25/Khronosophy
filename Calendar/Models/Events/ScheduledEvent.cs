
namespace Calendar.Models.Events;

public class ScheduledEvent(
    string name,
    DateTime startDateTime,
    DateTime endDateTime,
    Guid parentTaskId
) : IEvent
{
    public string Name { get; set; } = name;
    public DateTime StartDateTime { get; set; } = startDateTime;
    public DateTime EndDateTime { get; set; } = endDateTime;
    public Guid ParentTaskId { get; } = parentTaskId;

    public TimeSpan Duration
    {
        get => EndDateTime - StartDateTime;
        set => EndDateTime = StartDateTime + value;
    }
}
