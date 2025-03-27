
using Calendar.Models.Events;

namespace Calendar.Constants;

public static class ETFConstants
{
    // 24 hours in minutes...
    public const int MAX_MINIMUM_EVENT_DURATION_MINUTES = 1440;

    public const double WORKING_DAY_RATIO = 0.6666667d;
    public static readonly TimeSpan BREAK_DURATION = new(0, 30, 0);

    public const int MIN_TASK_IMPORTANCE = 1;
    public const int MAX_TASK_IMPORTANCE = 10;

    public const int MIN_TASK_INTENSITY = 1;
    public const int MAX_TASK_INTENSITY = 10;
}

