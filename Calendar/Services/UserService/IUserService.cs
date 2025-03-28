using ErrorOr;

using Calendar.Models;

namespace Calendar.Services.UserService;

public interface IUserService
{
    ErrorOr<Guid> GetDefaultUserGuid();
    ErrorOr<KhronosophyUser> GetUser(Guid userId);
    ErrorOr<Updated> AddUser(KhronosophyUser user);
    ErrorOr<Deleted> ClearScheduledEvents(KhronosophyUser user);
    ErrorOr<Deleted> ClearAllUsers();
}
