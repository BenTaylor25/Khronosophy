
using ErrorOr;

namespace Calendar.Models;

public class KhronosophyUser
{
    public Guid Id { get; set; }
    public Taskboard Taskboard { get; set; } = new();
    public EventCalendar EventCalendar { get; set; } = new();

    public int? DailyIntensityCapacity { get; set; }

    private KhronosophyUser(Guid id, int? dailyIntensityCapacity = null)
    {
        Id = id;
        DailyIntensityCapacity = dailyIntensityCapacity;
    }

    public static ErrorOr<KhronosophyUser> Create(
        Guid? id = null,
        int? dailyIntensityCapacity = null
    )
    {
        Guid guid = id ?? Guid.NewGuid();

        return new KhronosophyUser(guid, dailyIntensityCapacity);
    }
}
