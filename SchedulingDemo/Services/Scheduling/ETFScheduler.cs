
using SchedulingDemo.Models;

namespace SchedulingDemo.Services.Scheduling;

public class ETFScheduler : IScheduler
{
    // 8AM - 8PM
    private readonly TimeOnly DAY_START = new(8,0,0);
    private readonly TimeOnly DAY_END = new(20,0,0);

    private readonly double WORK_LEISURE_RATIO = 0.667;

    /// <summary>
    /// ETF Philosophy:
    /// Work hard early, enjoy the evening. <br />
    /// <br />
    /// ETF Method:
    /// Split the day into 3rds.
    /// The first 2/3rds of the day is dedicated to working in order of
    //. Importance. Q: WHAT DOES ETF SAY ABOUT BREAKS?
    /// The final 1/3rd cannot schedule events with an Intensity higher
    /// than 1. It's implied to be left empty (for the user to fill with
    /// leisurely static events manually).
    /// </summary>
    public void ScheduleUsersTasks(User user)
    {
        if (!AreParametersValid(user))
        {
            Console.WriteLine("User Invalid");
            return;   // Error message.
        }

        List<TaskboardTask> tasksByImportance =
            user.Taskboard.Tasks
                .OrderByDescending(task => task.Importance)
                .ToList();

        TimeSpan dayDuration = DAY_END - DAY_START;

        // Work Duration rounded to 15 minutes.
        TimeSpan workDuration = dayDuration * WORK_LEISURE_RATIO;
        workDuration = TimeSpan.FromMinutes(
            15 * Math.Floor(workDuration.TotalMinutes / 15)
        );

        TimeOnly workCutoffTime = DAY_START.Add(workDuration);
    }

    private bool AreParametersValid(User user)
    {
        if (user.DailyIntensityCapacity == null)
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
