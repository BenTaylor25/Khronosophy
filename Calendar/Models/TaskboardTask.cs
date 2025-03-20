using ErrorOr;

using Calendar.Models.Events;

namespace Calendar.Models;

public class TaskboardTask
{
    public Guid Id { get; }
    public string Name { get; set; }
    public TimeSpan ExpectedDuration { get; set; }

    public double? Importance { get; set; }
    public double? Intensity { get; set; }

    public List<ScheduledEvent> Events { get; set; } = [];

    private TaskboardTask(
        string name,
        TimeSpan expectedDuration,
        double? importance = null,
        double? intensity = null
    )
    {
        Id = Guid.NewGuid();
        Name = name;
        ExpectedDuration = expectedDuration;
        Importance = importance;
        Intensity = intensity;
    }

    public static ErrorOr<TaskboardTask> Create(
        string name,
        TimeSpan expectedDuration,
        double? importance = null,
        double? intensity = null
    )
    {
        bool taskInvalid =
            name.Length == 0 ||
            expectedDuration.TotalMinutes % 15 != 0 ||
            importance < 0 ||
            importance > 10 ||
            intensity < 0 ||
            intensity > 10;
        
        if (taskInvalid)
        {
            return Error.Validation();
        }

        return new TaskboardTask(
            name,
            expectedDuration,
            importance,
            intensity
        );
    }
}
