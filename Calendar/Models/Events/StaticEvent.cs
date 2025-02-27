using ErrorOr;

namespace Calendar.Models.Events;

public class StaticEvent : IEvent
{
    public string Name { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public TimeSpan Duration
    {
        get => EndDateTime - StartDateTime;
        set => EndDateTime = StartDateTime + value;
    }

    private StaticEvent(
        string name,
        DateTime startDateTime,
        DateTime endDateTime
    )
    {
        Name = name;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }

    public static ErrorOr<StaticEvent> Create(
        string name,
        DateTime startDateTime,
        DateTime endDateTime
    )
    {
        bool eventInvalid =
            name.Length == 0 ||
            startDateTime >= endDateTime;
            // TODO check datetimes divisible by 15 minutes

        if (eventInvalid)
        {
            return Error.Validation();
        }

        return new StaticEvent(name, startDateTime, endDateTime);
    }
}
