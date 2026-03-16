namespace SportsManagement.API.Models;

public class TeamStaff
{
    public int StaffId { get; set; }
    public int UserId { get; set; }
    public int TeamId { get; set; }
    public string StaffRole { get; set; } = string.Empty; // Head Coach, Assistant, Medic, Analyst

    public User User { get; set; } = null!;
    public Team Team { get; set; } = null!;
    public ICollection<CoachingPlan> CoachingPlans { get; set; } = new List<CoachingPlan>();
    public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
    public ICollection<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();
    public ICollection<TeamGameStats> AnalyzedStats { get; set; } = new List<TeamGameStats>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<FitnessRecord> FitnessRecords { get; set; } = new List<FitnessRecord>();
}
