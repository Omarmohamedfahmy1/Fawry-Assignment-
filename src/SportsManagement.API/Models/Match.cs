namespace SportsManagement.API.Models;

public class Match
{
    public int MatchId { get; set; }
    public int EventId { get; set; }
    public int? ScoreKeeperStaffId { get; set; }
    public string OpponentName { get; set; } = string.Empty;
    public int? TeamScore { get; set; }
    public int? OpponentScore { get; set; }
    public string? Result { get; set; } // Win, Loss, Draw

    public Event Event { get; set; } = null!;
    public TeamStaff? ScoreKeeperStaff { get; set; }
    public TeamGameStats? TeamGameStats { get; set; }
    public ICollection<PlayerGameStats> PlayerGameStats { get; set; } = new List<PlayerGameStats>();
    public ICollection<AiLineupSuggestion> AiLineupSuggestions { get; set; } = new List<AiLineupSuggestion>();
}
