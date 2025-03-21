
using Calendar.Models;
using ErrorOr;

namespace Calendar.Services.SampleDataService;

public interface ISampleDataService
{
    ErrorOr<Success> LoadTasksAndIntensities(
        KhronosophyUser user
    );
}
