
namespace Calendar.Controllers.RequestBodies;

public class UserControllerCreateBody(
    int? dailyIntesityCapacity
)
{
    public int? DailyIntensityCapacity { get; set; } =
        dailyIntesityCapacity;
}