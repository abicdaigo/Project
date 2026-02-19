using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class ProjectPhase : BaseEntity
{
    public int ProjectId { get; set; }
    public APQPPhase Phase { get; set; }
    public PhaseStatus Status { get; set; } = PhaseStatus.NotStarted;
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public decimal CompletionRate { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
    public ICollection<Deliverable> Deliverables { get; set; } = new List<Deliverable>();
}
