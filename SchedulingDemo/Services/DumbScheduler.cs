
using SchedulingDemo.Models;

namespace SchedulingDemo.Services;

public class DumbScheduler : IScheduler
{
    public void ScheduleUsersTasks(User user)
    {
        user.Calendar.Events.Add(new SchedulableEvent("one", DateTime.Now.AddHours(1), DateTime.Now.AddHours(2)));
        user.Calendar.Events.Add(new SchedulableEvent("two", DateTime.Now, DateTime.Now.AddHours(1)));
        user.Calendar.Events.Add(new SchedulableEvent("three", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1)));
    }
}

