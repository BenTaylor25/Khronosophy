
namespace SchedulingDemo.Models;

public class User(int? dailyIntensityCapacity = null)
{
    public Taskboard Taskboard { get; set; } = new Taskboard();
    public Calendar Calendar { get; set; } = new Calendar();

    public int? DailyIntensityCapacity { get; set; } =
        dailyIntensityCapacity;

}
