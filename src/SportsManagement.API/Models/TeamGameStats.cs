namespace SportsManagement.API.Models;

public class TeamGameStats
{
    public int ReportId { get; set; }
    public int MatchId { get; set; }
    public int? AnalystStaffId { get; set; }
    public string? SummaryPdfUrl { get; set; }
    public string? FullMatchVideoUrl { get; set; }
    public int PointsScored { get; set; }
    public int PointsConceded { get; set; }
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
    public float TeamEfficiency { get; set; }

    public Match Match { get; set; } = null!;
    public TeamStaff? AnalystStaff { get; set; }
}
