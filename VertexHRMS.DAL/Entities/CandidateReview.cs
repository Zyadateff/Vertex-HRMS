namespace VertexHRMS.DAL.Entities.Recruitment;

public class CandidateReview
{
    public int Id { get; set; }
    public int ATSCandidateId { get; set; }
    public string ReviewerUserId { get; set; } = default!;
    public CandidateStatus NewStatus { get; set; }
    public string? Notes { get; set; }
    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}