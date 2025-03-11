using ErrorOr;

using Calendar.Models;
using Calendar.Services.EventService;
using Calendar.Services.TaskboardService;
using Calendar.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Calendar.Services.SchedulingService.UTMTK;

namespace Calendar.Controllers;

public class SchedulingController : AppBaseController
{
    private readonly IUserService _userService;
    private readonly IUTMTKService _utmtkService;

    public SchedulingController(
        IUserService userService,
        IUTMTKService utmtkService
    )
    {
        _userService = userService;
        _utmtkService = utmtkService;
    }

    [HttpPost("/schedule/utmtk/{userId}")]
    public IActionResult UTMTKSchedule(Guid userId)
    {
        ErrorOr<KhronosophyUser> userServiceResponse =
            _userService.GetUser(userId);

        if (userServiceResponse.IsError)
        {
            return Problem("User does not exist.");
        }
        KhronosophyUser user = userServiceResponse.Value;

        ErrorOr<Success> utmtkServiceResponse =
            _utmtkService.ScheduleEvents(user);

        if (utmtkServiceResponse.IsError)
        {
            return Problem();
        }
        return Ok();
    }
}
