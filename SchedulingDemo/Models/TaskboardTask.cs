
namespace SchedulingDemo.Models;

public class TaskboardTask(
    string name,
    TimeSpan expectedDuration,
    double? importance = null,
    double? intensity = null
)
{
    public string Name { get; set; } = name;
    public TimeSpan ExpectedDuration { get; set; } = expectedDuration;
    public List<ScheduledEvent> Events { get; set; } = [];
    public double? Importance { get; set; } = importance;
    public double? Intensity { get; set; } = intensity;
}

