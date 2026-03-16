namespace SportsManagement.API.Models;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int EventId { get; set; }
    public int PlayerId { get; set; }
    public int RecordedByStaffId { get; set; }
    public string Status { get; set; } = string.Empty; // Present, Absent, Late, Injured

    public Event Event { get; set; } = null!;
    public PlayerProfile Player { get; set; } = null!;
    public TeamStaff RecordedByStaff { get; set; } = null!;
}
