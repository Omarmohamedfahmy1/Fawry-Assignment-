namespace SportsManagement.API.Models;

public class Announcement
{
    public int AnnouncementId { get; set; }
    public int TeamId { get; set; }
    public int CreatorUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Team Team { get; set; } = null!;
    public User Creator { get; set; } = null!;
}
