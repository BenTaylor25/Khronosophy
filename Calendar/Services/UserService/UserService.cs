using ErrorOr;

using Calendar.Models;
using Calendar.Models.Events;

namespace Calendar.Services.UserService;

public class UserService : IUserService
{
    private Dictionary<Guid, KhronosophyUser> Users { get; set; } = [];

    public UserService()
    {
        // Get list of users from database.
    }
    
    public ErrorOr<Guid> GetDefaultUserGuid()
    {
        if (Users.Count == 0)
        {
            return Error.NotFound();
        }

        KhronosophyUser defaultUser = Users.First().Value;

        return defaultUser.Id;
    }

    public ErrorOr<KhronosophyUser> GetUser(Guid userId)
    {
        if (Users.ContainsKey(userId))
        {
            return Users[userId];
        }
        return Error.NotFound();
    }

    public ErrorOr<Updated> AddUser(KhronosophyUser user)
    {
        Users.Add(user.Id, user);
        return Result.Updated;
    }

    public ErrorOr<Deleted> ClearScheduledEvents(KhronosophyUser user)
    {
        // Events.
        int listLen = user.EventCalendar.Events.Count;

        for (int i = listLen - 1; i >= 0; i--)
        {
            IEvent calendarEvent = user.EventCalendar.Events[i];

            if (calendarEvent is ScheduledEvent scheduledEvent)
            {
                user.EventCalendar.Events.RemoveAt(i);
            }
        }

        // Tasks.
        foreach (TaskboardTask taskboardTask in user.Taskboard.Tasks)
        {
            taskboardTask.Events = [];
        }

        return new Deleted();
    }

    public ErrorOr<Deleted> ClearAllUsers()
    {
        // The GC will clean the objects up right?
        Users = [];
        return Result.Deleted;
    }
}
