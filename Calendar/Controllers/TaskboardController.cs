using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using Calendar.Services.TaskboardService;
using Calendar.Services.UserService;
using Calendar.Models;
using Calendar.Controllers.RequestBodies;

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

    [HttpPost("/taskboard")]
    public IActionResult AddTask(
        [FromBody] TaskControllerAddBody requestBody
    )
    {
        ErrorOr<KhronosophyUser> userServiceResponse =
            _userService.GetUser(requestBody.UserId);

        if (userServiceResponse.IsError)
        {
            return Problem("User does not exist");
        }
        KhronosophyUser user = userServiceResponse.Value;

        ErrorOr<TaskboardTask> taskResponse = TaskboardTask.Create(
            requestBody.Name,
            TimeSpan.FromMinutes(requestBody.ExpectedDurationMinutes),
            requestBody.Importance,
            requestBody.Intensity
        );

        if (taskResponse.IsError)
        {
            return Problem("Could not create task.");
        }
        TaskboardTask task = taskResponse.Value;

        ErrorOr<Success> taskboardServiceResponse =
            _taskboardService.AddTaskToUser(user, task);

        if (taskboardServiceResponse.IsError)
        {
            return Problem("Could not add task to user.");
        }

        return Ok(task);
    }
}
