using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class Milestone : BaseEntity
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ActualDate { get; set; }
    public MilestoneStatus Status { get; set; } = MilestoneStatus.Pending;
    public APQPPhase Phase { get; set; }
    public int SortOrder { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
