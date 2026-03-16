namespace SportsManagement.API.Models;

public class MedicalRecord
{
    public int RecordId { get; set; }
    public int PlayerId { get; set; }
    public int DoctorStaffId { get; set; }
    public DateTime RecordDate { get; set; }
    public string? InjuryType { get; set; }
    public string? Diagnosis { get; set; }
    public string? ReportFileUrl { get; set; }
    public DateOnly? ExpectedReturnDate { get; set; }

    public PlayerProfile Player { get; set; } = null!;
    public TeamStaff DoctorStaff { get; set; } = null!;
}
