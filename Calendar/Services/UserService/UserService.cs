using ErrorOr;

using Calendar.Models;

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
}
