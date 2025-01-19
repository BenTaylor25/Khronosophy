
namespace SchedulingDemo.Models.Enums;

public enum Intensity
{
    Low,
    Medium,
    High,
}

public static class IntensityHelper
{
    private const string LOW = "low";
    private const string MEDIUM = "medium";
    private const string HIGH = "high";

    public static Intensity Parse(string intensity) =>
        intensity.ToLower() switch
        {
            LOW => Intensity.Low,
            MEDIUM => Intensity.Medium,
            HIGH => Intensity.High,
            _ => throw new ArgumentException($"Invalid intensity: {intensity}"),
        };

    public static Intensity? ParseNullable(string? intensity) =>
        intensity != null ? Parse(intensity) : null;

    public static string ToString(Intensity intensity) =>
        intensity switch
        {
            Intensity.Low => LOW,
            Intensity.Medium => MEDIUM,
            Intensity.High => HIGH,
            _ => throw new ArgumentException($"Invalid intensity: {intensity}"),
        };
}
