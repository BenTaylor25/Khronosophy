
using Calendar.Services.UserService;
using Calendar.Services.EventService;
using Calendar.Services.TaskboardService;

namespace Calendar.Controllers;

public class EventController : AppBaseController
{
    private readonly IUserService _userService;
    private readonly IEventService _eventService;
    private readonly ITaskboardService _taskboardService;

    public EventController(
        IUserService userService,
        IEventService eventService,
        ITaskboardService taskboardService
    )
    {
        _userService = userService;
        _eventService = eventService;
        _taskboardService = taskboardService;
    }
}
