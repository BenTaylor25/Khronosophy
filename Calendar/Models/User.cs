
namespace Calendar.Models;

public class User(int? dailyIntensityCapacity = null)
{
    public Taskboard Taskboard { get; set; } = new();
    public EventCalendar EventCalendar { get; set; } = new();

    public int? DailyIntensityCapacity { get; set; } =
        dailyIntensityCapacity;
}
