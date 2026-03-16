namespace SportsManagement.API.Models;

public class FitnessRecord
{
    public int FitnessId { get; set; }
    public int PlayerId { get; set; }
    public int FitnessStaffId { get; set; }
    public DateTime TestDate { get; set; }
    public float? Bmi { get; set; }
    public float? BodyFatPercentage { get; set; }
    public float? SpeedTestResult { get; set; }
    public float? EnduranceScore { get; set; }

    public PlayerProfile Player { get; set; } = null!;
    public TeamStaff FitnessStaff { get; set; } = null!;
}
