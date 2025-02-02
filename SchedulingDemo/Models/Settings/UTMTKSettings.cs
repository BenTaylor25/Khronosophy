
namespace SchedulingDemo.Models.Settings;

public record UTMTKSettings
{
    public int BreakAfterHighIntensityTask { get; set; }
    public int BreakAfterMediumIntensityTask { get; set; }
    public int BreakAfterLowIntensityTask { get; set; }

    public int HighIntensityValue { get; set; }
    public int MediumIntensityValue { get; set; }
    public int LowIntensityValue { get; set; }
}
