using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class GateReview : BaseEntity
{
    public int ProjectId { get; set; }
    public int GateNumber { get; set; }
    public DateTime? ReviewDate { get; set; }
    public GateResult Result { get; set; } = GateResult.NotReviewed;
    public string? ReviewerId { get; set; }
    public string? ReviewerName { get; set; }
    public string? Notes { get; set; }
    public string? ActionItems { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
