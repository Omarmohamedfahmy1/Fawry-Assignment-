namespace SportsManagement.API.Models;

public class CoachingPlan
{
    public int PlanId { get; set; }
    public int CreatorStaffId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DrillFilesUrl { get; set; }

    public TeamStaff CreatorStaff { get; set; } = null!;
    public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
}
