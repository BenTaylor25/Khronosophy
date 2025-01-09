
namespace KhronosScheduling.Models;

public class Task(
    string name,
    TimeSpan expectedDuration
)
{
    public string Name { get; set; } = name;
    public TimeSpan ExpectedDuration = expectedDuration;
}

