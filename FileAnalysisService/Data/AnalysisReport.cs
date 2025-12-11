public class AnalysisReport
{
    public Guid Id { get; set; }

    public Guid CheckedFileId { get; set; }
    public Guid? MatchedFileId { get; set; }

    public double MaxSimilarityPercent { get; set; }
    public bool IsPlagiarism { get; set; }
    public DateTime CheckedAt { get; set; }

    public string StudentName { get; set; } = string.Empty;
    public string AssignmentName { get; set; } = string.Empty;
}
