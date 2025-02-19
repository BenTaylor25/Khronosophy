
namespace SchedulingDemo.Models;

public class Calendar
{
    public List<IEvent> Events { get; set; }

    public Calendar()
    {
        Events = [];
    }

    public void Print()
    {
        var sortedEvents =
            from e in Events
            orderby e.StartDateTime.Ticks
            select e;

        DateOnly? previousDate = null;

        foreach (IEvent calendarEvent in sortedEvents)
        {
            DateOnly todaysDate =
                DateOnly.FromDateTime(calendarEvent.StartDateTime);

            if (previousDate != todaysDate)
            {
                previousDate = todaysDate;
                Console.WriteLine();
                Console.WriteLine(previousDate);
            }

            string startTime =
                TimeOnly
                    .FromDateTime(calendarEvent.StartDateTime)
                    .ToString();

            string endTime =
                TimeOnly
                    .FromDateTime(calendarEvent.EndDateTime)
                    .ToString();

            Console.WriteLine(
                $"{calendarEvent.Name}   :   {startTime} - {endTime}"
            );
        }
    }
}
