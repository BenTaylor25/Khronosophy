using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using Calendar.Models;
using Calendar.Models.Events;
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

    [HttpGet("/events/{userId}")]
    public IActionResult GetAllEvents(Guid userId)
    {
        ErrorOr<KhronosophyUser> userServiceResponse =
            _userService.GetUser(userId);

        if (userServiceResponse.IsError)
        {
            return Problem("User does not exist");
        }
        KhronosophyUser user = userServiceResponse.Value;

        ErrorOr<List<IEvent>> eventServiceResponse =
            _eventService.GetUserEvents(user);

        if (eventServiceResponse.IsError)
        {
            return Problem("Could not retreive user's events.");
        }
        List<IEvent> userEvents = eventServiceResponse.Value;

        return Ok(userEvents);
    }

}
