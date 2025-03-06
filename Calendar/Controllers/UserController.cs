using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using Calendar.Models;
using Calendar.Services.UserService;

namespace Calendar.Controllers;

public class UserController : AppBaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// This is for simplification of the research project.
    /// Should this be evolved to a commercial product, this
    /// should be removed.
    /// </summary>
    [HttpGet("user/getDefaultUserId")]
    public IActionResult GetDefaultUserId()
    {
        ErrorOr<Guid> serviceResponse = _userService.GetDefaultUserGuid();

        if (serviceResponse.IsError)
        {
            return Problem("Database not populated.");
        }
        Guid id = serviceResponse.Value;

        return Ok(new { id = id });
    }

    [HttpGet("/user/{id}")]
    public IActionResult GetUser(Guid id)
    {
        ErrorOr<KhronosophyUser> serviceResponse = _userService.GetUser(id);

        if (serviceResponse.IsError)
        {
            return Problem("User does not exist.");
        }
        return Ok(serviceResponse.Value);
    }

    [HttpPost("/user")]
    public IActionResult CreateUser()
    {
        ErrorOr<KhronosophyUser> userResponse = KhronosophyUser.Create();

        if (userResponse.IsError)
        {
            return Problem("Could not create User.");
        }
        KhronosophyUser user = userResponse.Value;

        ErrorOr<Updated> createUserResponse =
            _userService.AddUser(user);
        
        if (createUserResponse.IsError)
        {
            return Problem("Could not add User to system.");
        }
        return Ok(user);
    }
}
