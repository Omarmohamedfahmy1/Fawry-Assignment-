namespace SportsManagement.API.Models;

public class TrainingSession
{
    public int SessionId { get; set; }
    public int EventId { get; set; }
    public int? PlanId { get; set; }
    public int ConductedByStaffId { get; set; }
    public string? SessionNotes { get; set; }

    public Event Event { get; set; } = null!;
    public CoachingPlan? Plan { get; set; }
    public TeamStaff ConductedByStaff { get; set; } = null!;
}
