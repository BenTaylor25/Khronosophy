using SchedulingDemo.Models;
using SchedulingDemo.Services;

User user = LoadSampleData.GetUserWithTasks();

foreach (var task in user.Taskboard.Tasks)
{
    Console.WriteLine($"{task.Name}: ~ {task.ExpectedDuration.Hours}hrs");
}

Console.WriteLine();
Console.WriteLine("---");

DumbScheduler scheduler = new();
scheduler.ScheduleUsersTasks(user);

user.Calendar.Print();
