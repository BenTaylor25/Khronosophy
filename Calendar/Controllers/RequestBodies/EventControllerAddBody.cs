
namespace Calendar.Controllers.RequestBodies;

public class EventControllerAddBody(
    Guid userId,
    string name,
    DateTime startDateTime,
    DateTime endDateTime
)
{
    public Guid UserId { get; } = userId;
    public string Name { get; } = name;
    public DateTime StartDateTime { get; } = startDateTime;
    public DateTime EndDateTime { get; } = endDateTime;
}
