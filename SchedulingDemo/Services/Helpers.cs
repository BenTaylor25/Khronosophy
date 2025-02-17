
namespace SchedulingDemo.Services;

public static class Helpers
{
    /// <summary>
    /// Rounds up to the next whole 15 minutes.
    /// </summary>
    public static double TimeSpanUpToDecimalHours(TimeSpan timeSpan)
    {
        double hoursComponent = timeSpan.Hours;

        // Ceil(minutes / 15) * 15 rounds up to the next whole 15 minutes.
        // From there, divide by 60 for hours.
        // Optimised by skipping straight to hours after ceil.
        double minutesComponent = Math.Ceiling(timeSpan.Minutes / 15d) / 4;

        return hoursComponent + minutesComponent;
    }

    /// <summary>
    /// Rounds down to the previous whole 15 minutes.
    /// </summary>
    public static double TimeSpanDownToDecimalHours(TimeSpan timeSpan)
    {
        double hoursComponent = timeSpan.Hours;

        // Floor(minutes / 15) * 15 rounds down to the previous whole 15
        // minutes.
        // From there, divide by 60 for hours.
        // Optimised by skipping straight to hours after ceil.
        double minutesComponent = Math.Floor(timeSpan.Minutes / 15d) / 4;

        return hoursComponent + minutesComponent;
    }

    /// <summary>
    /// Rounds a TimeSpan up to the next whole 15 minutes.
    /// </summary>
    public static TimeSpan TimeSpanToNextFifteenMinutes(TimeSpan timeSpan)
    {
        double minutesWithDecimalSeconds =
            timeSpan.Minutes + (timeSpan.Seconds / 60);

        int roundedMinutes =
            (int)Math.Ceiling(minutesWithDecimalSeconds / 15) * 15;

        return new TimeSpan(
            timeSpan.Days,
            timeSpan.Hours,
            roundedMinutes,
            0
        );
    }

    /// <summary>
    /// Rounds a TimeSpan down to the previous whole 15 minutes.
    /// </summary>
    public static TimeSpan TimeSpanToPreviousFifteenMinutes(
        TimeSpan timeSpan
    )
    {
        double minutesWithDecimalSeconds =
            timeSpan.Minutes + (timeSpan.Seconds / 60);

        int roundedMinutes =
            (int)Math.Floor(minutesWithDecimalSeconds / 15) * 15;

        return new TimeSpan(
            timeSpan.Days,
            timeSpan.Hours,
            roundedMinutes,
            0
        );
    }
}
