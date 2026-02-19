using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class Issue : BaseEntity
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IssuePriority Priority { get; set; } = IssuePriority.Medium;
    public IssueStatus Status { get; set; } = IssueStatus.Open;
    public IssueCategory Category { get; set; } = IssueCategory.Other;
    public string? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public string? Resolution { get; set; }
    public string? RootCause { get; set; }
    public APQPPhase? RelatedPhase { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
