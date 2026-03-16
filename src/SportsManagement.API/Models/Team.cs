namespace SportsManagement.API.Models;

public class Team
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // U16, U18, Senior
    public int ManagerUserId { get; set; }

    public User Manager { get; set; } = null!;
    public ICollection<TeamStaff> Staff { get; set; } = new List<TeamStaff>();
    public ICollection<PlayerProfile> Players { get; set; } = new List<PlayerProfile>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    public ICollection<TeamCumulativeStats> CumulativeStats { get; set; } = new List<TeamCumulativeStats>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
