using SchedulingDemo.Models;

namespace SchedulingDemo.Services;

public interface IScheduler
{
    void ScheduleUsersTasks(User user);
}
