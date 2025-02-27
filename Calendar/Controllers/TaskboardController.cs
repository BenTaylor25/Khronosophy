using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using Calendar.Services.Taskboard;
using Calendar.Services.UserService;
using Calendar.Models;

namespace Calendar.Controllers;

public class TaskboardController : AppBaseController
{
    private readonly IUserService _userService;
    private readonly ITaskboardService _taskboardService;

    public TaskboardController(
        IUserService userService,
        ITaskboardService taskboardService
    )
    {
        _userService = userService;
        _taskboardService = taskboardService;
    }

    [HttpGet("/taskboard/{userId}")]
    public IActionResult GetAllTasks(Guid userId)
    {
        ErrorOr<KhronosophyUser> userServiceResponse =
            _userService.GetUser(userId);

        if (userServiceResponse.IsError)
        {
            return Problem("User does not exist");
        }
        KhronosophyUser user = userServiceResponse.Value;

        ErrorOr<List<TaskboardTask>> taskboardServiceResponse =
            _taskboardService.GetUserTasks(user);

        if (taskboardServiceResponse.IsError)
        {
            return Problem("Could not retreive user's tasks.");
        }
        List<TaskboardTask> userTasks = taskboardServiceResponse.Value;

        return Ok(userTasks);
    }

}
