using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class Deliverable : BaseEntity
{
    public int ProjectPhaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DeliverableStatus Status { get; set; } = DeliverableStatus.NotStarted;
    public DateTime? DueDate { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public DateTime? CompletedDate { get; set; }
    public bool Required { get; set; } = true;

    // Navigation
    public ProjectPhase ProjectPhase { get; set; } = null!;
}
