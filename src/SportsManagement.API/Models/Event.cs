namespace SportsManagement.API.Models;

public class Event
{
    public int EventId { get; set; }
    public int TeamId { get; set; }
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = string.Empty; // Match, Training, Meeting

    public Team Team { get; set; } = null!;
    public Match? Match { get; set; }
    public TrainingSession? TrainingSession { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
