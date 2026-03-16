namespace SportsManagement.API.Models;

public class AiLineupSuggestion
{
    public int SuggestionId { get; set; }
    public int MatchId { get; set; }
    public string SuggestedLineup { get; set; } = "{}"; // stored as JSON string
    public string? AiReasoning { get; set; }
    public float ConfidenceScore { get; set; }

    public Match Match { get; set; } = null!;
}
