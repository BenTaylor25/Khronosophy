
namespace SchedulingDemo.Models;

public class User
{
    public Taskboard Taskboard { get; set; }
    public Calendar Calendar { get; set; }

    public User()
    {
        Taskboard = new Taskboard();
        Calendar = new Calendar();
    }
}
