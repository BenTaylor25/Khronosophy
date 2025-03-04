
namespace Calendar.Models.Events;

/// <summary>
/// A Task is purely a goal to be completed with no time associated with
/// it. An Event is a full time block associated with a Task.
/// An EventRequest is an unscheduled instance of a Task with a desired
/// duration but no start or end time. EventRequests are used only as
/// temporary helper objects in complex scheduling algorithms.
/// </summary>
public record EventRequest(
    TaskboardTask? ParentTask,
    TimeSpan Duration
)
{
    /// <summary>
    /// When ParentTask is null, the EventRequest is actually requesting
    /// extra down time.
    /// </summary>
    public TaskboardTask? ParentTask { get; } = ParentTask;

    public TimeSpan Duration { get; } = Duration;

    public double Intesity => ParentTask?.Intensity ?? 0;
}
