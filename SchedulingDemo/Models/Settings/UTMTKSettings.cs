
namespace SchedulingDemo.Models.Settings;

public record UTMTKSettings(
    int BreakAfterHighIntensityTask,
    int BreakAfterMediumIntensityTask,
    int BreakAfterLowIntensityTask,
    int HighIntensityValue,
    int MediumIntensityValue,
    int LowIntensityValue
)
{
    public int BreakAfterHighIntensityTask { get; set; } =
        BreakAfterHighIntensityTask;
    public int BreakAfterMediumIntensityTask { get; set; } =
        BreakAfterMediumIntensityTask;
    public int BreakAfterLowIntensityTask { get; set; } =
        BreakAfterLowIntensityTask;

    public int HighIntensityValue { get; set; } =
        HighIntensityValue;
    public int MediumIntensityValue { get; set; } =
        MediumIntensityValue;
    public int LowIntensityValue { get; set; } =
        LowIntensityValue;
}
