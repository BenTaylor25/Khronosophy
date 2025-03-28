using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using Calendar.Models;
using Calendar.Services.UserService;
using Calendar.Services.SchedulingService.DumbScheduler;
using Calendar.Services.SchedulingService.ETF;
using Calendar.Services.SchedulingService.UTMTK;

namespace Calendar.Controllers;

public class SchedulingController : AppBaseController
{
    private readonly IUserService _userService;
    private readonly IDumbSchedulerService _dumbSchedulerService;
    private readonly IETFService _etfService;
    private readonly IUTMTKService _utmtkService;

    public SchedulingController(
        IUserService userService,
        IDumbSchedulerService dumbSchedulerService,
        IETFService etfService,
        IUTMTKService utmtkService
    )
    {
        _userService = userService;
        _dumbSchedulerService = dumbSchedulerService;
        _etfService = etfService;
        _utmtkService = utmtkService;
    }

    [HttpPost("/schedule/dumbscheduler/{userId}")]
    public IActionResult DumbSchedule(Guid userId)
    {
        ErrorOr<KhronosophyUser> userServiceResponse =
            _userService.GetUser(userId);

        if (userServiceResponse.IsError)
        {
            return Problem("User does not exist.");
        }
        KhronosophyUser user = userServiceResponse.Value;

        ErrorOr<Success> dumbSchedulerServiceResponse =
            _dumbSchedulerService.ScheduleEvents(user);

        if (dumbSchedulerServiceResponse.IsError)
        {
            return Problem();
        }
        return Ok();
    }

    [HttpPost("/schedule/etf/{userId}")]
    public IActionResult ETFSchedule(Guid userId)
    {
        ErrorOr<KhronosophyUser> userServiceResponse =
            _userService.GetUser(userId);

        if (userServiceResponse.IsError)
        {
            return Problem("User does not exist.");
        }
        KhronosophyUser user = userServiceResponse.Value;

        ErrorOr<Success> etfServiceResponse =
            _etfService.ScheduleEvents(user);

        if (etfServiceResponse.IsError)
        {
            return Problem();
        }
        return Ok();
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
            string errorMessage = "UTMTK Service Failed: ";

            foreach (Error error in utmtkServiceResponse.Errors)
            {
                errorMessage += error.Description;
            }

            return Problem(errorMessage);
        }
        return Ok();
    }

    [HttpDelete("/schedule/unscheduleTasks/{userId}")]
    public IActionResult UnscheduleTasks(Guid userId)
    {
        ErrorOr<KhronosophyUser> userServiceResponse =
            _userService.GetUser(userId);

        if (userServiceResponse.IsError)
        {
            return Problem("User does not exist.");
        }
        KhronosophyUser user = userServiceResponse.Value;

        ErrorOr<Deleted> userServiceClearScheduleResponse =
            _userService.ClearScheduledEvents(user);

        if (userServiceClearScheduleResponse.IsError)
        {
            return Problem("Failed to clear schedule");
        }
        return Ok();
    }
}
