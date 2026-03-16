using SportsManagement.API.Enums;

namespace SportsManagement.API.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? PasswordHash { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }

    // Google OAuth fields
    public string? GoogleId { get; set; }
    public bool IsGoogleUser { get; set; } = false;

    // Approval workflow
    public UserStatus Status { get; set; } = UserStatus.PendingApproval;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation
    public User? ApprovedBy { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Team> ManagedTeams { get; set; } = new List<Team>();
    public ICollection<TeamStaff> StaffRoles { get; set; } = new List<TeamStaff>();
    public PlayerProfile? PlayerProfile { get; set; }
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
}
