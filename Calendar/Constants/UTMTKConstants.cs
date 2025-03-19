
using Calendar.Models.Events;

namespace Calendar.Constants;

public static class UTMTKConstants
{
    // Do we need this? Value needs to be revised if yes.
    public const int MAX_DAILY_INTENSITY_CAPACITY = 100;

    // 24 hours in minutes...
    public const int MAX_MINIMUM_EVENT_DURATION_MINUTES = 1440;

    public const int MIN_TASK_IMPORTANCE = 1;
    public const int MAX_TASK_IMPORTANCE = 10;

    public const int MIN_TASK_INTENSITY = 1;
    public const int MAX_TASK_INTENSITY = 10;

    public const int BREAK_DURATION = 1;
    public static readonly EventRequest EVENT_REQUEST_BREAK =
        new(null, TimeSpan.FromHours(BREAK_DURATION));
}
