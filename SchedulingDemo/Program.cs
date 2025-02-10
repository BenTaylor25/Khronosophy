using SchedulingDemo.Models;
using SchedulingDemo.Models.Settings;
using SchedulingDemo.Services;
using SchedulingDemo.Services.Scheduling;

User user = LoadSampleData.GetUserWithTasksAndIntensities();

foreach (var task in user.Taskboard.Tasks)
{
    Console.WriteLine($"{task.Name}: ~ {task.ExpectedDuration.Hours}hrs");
}

Console.WriteLine();
Console.WriteLine("---");

UTMTKSettings settings = new(3, 2, 1, 10, 5, 2);

IScheduler scheduler = new UTMTKScheduler(settings);
scheduler.ScheduleUsersTasks(user);

user.Calendar.Print();
