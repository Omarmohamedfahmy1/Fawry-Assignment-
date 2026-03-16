namespace SportsManagement.API.Models;

public class PlayerGameStats
{
    public int GameStatId { get; set; }
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public int MinutesPlayed { get; set; }
    public int Points { get; set; }
    public int Rebounds { get; set; }
    public int Assists { get; set; }
    public int Steals { get; set; }
    public int Blocks { get; set; }
    public int Turnovers { get; set; }
    public int PersonalFouls { get; set; }
    public int FieldGoalsMade { get; set; }
    public int FieldGoalsAttempted { get; set; }
    public int ThreePointsMade { get; set; }
    public int FreeThrowsMade { get; set; }
    public float GameEfficiencyRating { get; set; }
    public string? ClipsVideoUrl { get; set; }

    public Match Match { get; set; } = null!;
    public PlayerProfile Player { get; set; } = null!;
}
