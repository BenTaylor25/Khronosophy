
namespace SchedulingDemo.Models;

public class TaskboardTask(
    string name,
    TimeSpan expectedDuration
)
{
    public string Name { get; set; } = name;
    public TimeSpan ExpectedDuration = expectedDuration;
}

