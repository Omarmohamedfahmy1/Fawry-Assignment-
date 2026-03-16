namespace SportsManagement.API.Models;

public class TeamCumulativeStats
{
    public int TeamStatId { get; set; }
    public int TeamId { get; set; }
    public string SeasonYear { get; set; } = string.Empty; // e.g., 2025-2026
    public int MatchesPlayed { get; set; }
    public int MatchesStarted { get; set; }
    public int TotalPointsScored { get; set; }
    public int TotalPointsConceded { get; set; }
    public int TotalRebounds { get; set; }
    public int TotalTurnovers { get; set; }
    public int TotalFouls { get; set; }
    public int TotalAssists { get; set; }
    public int TotalSteals { get; set; }
    public int TotalBlocks { get; set; }
    public int TotalFieldGoalsMade { get; set; }
    public int TotalFieldGoalsAttempted { get; set; }
    public int TotalThreePointsMade { get; set; }
    public int TotalFreeThrowsMade { get; set; }
    public float Per { get; set; } // Team Efficiency Rating

    public Team Team { get; set; } = null!;
}
