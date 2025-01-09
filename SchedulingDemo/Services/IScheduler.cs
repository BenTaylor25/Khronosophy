using KhronosScheduling.Models;

namespace KhronosScheduling.Services;

public interface IScheduler
{
    void ScheduleUsersTasks(User user);
}
