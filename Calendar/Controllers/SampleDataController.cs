using Microsoft.AspNetCore.Mvc;

using ErrorOr;

using Calendar.Models;
using Calendar.Controllers;
using Calendar.Services.UserService;
using Calendar.Services.SampleDataService;

public class SampleDataController : AppBaseController
{
    private readonly IUserService _userService;
    private readonly ISampleDataService _sampleDataService;

    public SampleDataController(
        IUserService userService,
        ISampleDataService sampleDataService
    )
    {
        _userService = userService;
        _sampleDataService = sampleDataService;
    }

    [HttpDelete("/sampleData/clearAll")]
    public IActionResult ClearAllData()
    {
        _userService.ClearAllUsers();
        return Ok();
    }

    [HttpPost("/sampleData/loadWithTasksAndIntensities")]
    public IActionResult LoadUserWithTasksAndIntensities()
    {
        ErrorOr<KhronosophyUser> userResponse = KhronosophyUser.Create();

        if (userResponse.IsError)
        {
            return Problem("Could not create User.");
        }
        KhronosophyUser user = userResponse.Value;

        _userService.AddUser(user);
        _sampleDataService.LoadTasksAndIntensities(user);

        return Ok();
    }
}
