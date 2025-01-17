

namespace SchedulingDemo.Models;

public class SchedulableEvent(
    string name,
    DateTime startDateTime,
    DateTime endDateTime,
    TaskboardTask parentTask
) : IEvent
{
    public string Name { get; set; } = name;
    public DateTime StartDateTime { get; set; } = startDateTime;
    public DateTime EndDateTime { get; set; } = endDateTime;
    public TaskboardTask ParentTask { get; set;} = parentTask;
    // public bool IsScheduled = false;

    public TimeSpan Duration
    {
        get => EndDateTime - StartDateTime;
        set => EndDateTime = StartDateTime + value;
    }
}

