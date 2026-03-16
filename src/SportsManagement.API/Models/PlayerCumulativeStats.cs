namespace SportsManagement.API.Models;

public class PlayerCumulativeStats
{
    public int PlayerStatId { get; set; }
    public int PlayerId { get; set; }
    public string SeasonYear { get; set; } = string.Empty;
    public int MatchesPlayed { get; set; }
    public int MatchesStarted { get; set; }
    public int TotalMinutesPlayed { get; set; }
    public int TotalPoints { get; set; }
    public int TotalRebounds { get; set; }
    public int TotalAssists { get; set; }
    public int TotalSteals { get; set; }
    public int TotalBlocks { get; set; }
    public int TotalTurnovers { get; set; }
    public int TotalPersonalFouls { get; set; }
    public int TotalFieldGoalsMade { get; set; }
    public int TotalFieldGoalsAttempted { get; set; }
    public int TotalThreePointsMade { get; set; }
    public int TotalFreeThrowsMade { get; set; }
    public float TotalGamesEfficiencyRating { get; set; }

    public PlayerProfile Player { get; set; } = null!;
}
