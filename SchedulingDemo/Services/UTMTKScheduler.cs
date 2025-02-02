using SchedulingDemo.Models;
using SchedulingDemo.Models.Settings;

namespace SchedulingDemo.Services;

public class UTMTKScheduler(UTMTKSettings settings) : IScheduler
{
    private UTMTKSettings Settings { get; set; } = settings;

    public void ScheduleUsersTasks(User user)
    {
        if (!AreParametersValid(user))
        {
            return;   // Error message.
        }

        List<TaskboardTask> tasksByImportance =
            user.Taskboard.Tasks.OrderBy(task => task.Importance).ToList();
    }

    private bool AreParametersValid(User user)
    {
        if (user.DailyIntensityCapacity == null)
        {
            return false;
        }

        bool highIntensityGreaterThanDailyCapacity =
            Settings.HighIntensityValue > user.DailyIntensityCapacity;

        bool intensityValuesNotInOrder =
            Settings.HighIntensityValue < Settings.MediumIntensityValue ||
            Settings.MediumIntensityValue < Settings.LowIntensityValue;

        if (
            highIntensityGreaterThanDailyCapacity ||
            intensityValuesNotInOrder
        )
        {
            return false;
        }

        foreach (TaskboardTask task in user.Taskboard.Tasks)
        {
            if (
                task.Importance == null ||
                task.Intensity == null
            )
            {
                return false;
            }
        }

        return true;
    }
}

