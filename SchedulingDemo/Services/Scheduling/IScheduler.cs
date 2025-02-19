using SchedulingDemo.Models;

namespace SchedulingDemo.Services.Scheduling;

public interface IScheduler
{
    void ScheduleUsersTasks(User user);
}
