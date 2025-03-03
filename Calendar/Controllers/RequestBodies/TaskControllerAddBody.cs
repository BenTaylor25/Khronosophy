
namespace Calendar.Controllers.RequestBodies;

public class TaskControllerAddBody(
    Guid userId,
    string name,
    int expectedDurationMinutes,
    int? importance,
    int? intensity
)
{
    public Guid UserId { get; } = userId;
    public string Name { get; } = name;
    public int ExpectedDurationMinutes { get; } = expectedDurationMinutes;
    public int? Importance { get; } = importance;
    public int? Intensity { get; } = intensity;
}
