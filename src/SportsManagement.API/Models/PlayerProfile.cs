namespace SportsManagement.API.Models;

public class PlayerProfile
{
    public int PlayerId { get; set; }
    public int UserId { get; set; }
    public int TeamId { get; set; }
    public string? Position { get; set; }
    public float? Height { get; set; }
    public float? Weight { get; set; }
    public DateOnly? Dob { get; set; }

    public User User { get; set; } = null!;
    public Team Team { get; set; } = null!;
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<PlayerGameStats> GameStats { get; set; } = new List<PlayerGameStats>();
    public ICollection<PlayerCumulativeStats> CumulativeStats { get; set; } = new List<PlayerCumulativeStats>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<FitnessRecord> FitnessRecords { get; set; } = new List<FitnessRecord>();
}
