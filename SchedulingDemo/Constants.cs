using SchedulingDemo.Models;

namespace SchedulingDemo;

public static class Constants
{
    public static readonly EventRequest EVENT_REQUEST_BREAK =
        new(null, TimeSpan.FromHours(1));
}
