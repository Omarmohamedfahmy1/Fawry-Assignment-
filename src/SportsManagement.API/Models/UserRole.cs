namespace SportsManagement.API.Models;

public class UserRole
{
    public int UserRoleId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public int? TeamId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public int AssignedByUserId { get; set; }

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public Team? Team { get; set; }
    public User AssignedBy { get; set; } = null!;
}
