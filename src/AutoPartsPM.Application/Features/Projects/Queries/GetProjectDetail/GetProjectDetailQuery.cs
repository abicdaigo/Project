using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Queries.GetProjectDetail;

public record GetProjectDetailQuery(int Id) : IRequest<ProjectDetailDto?>;

public record ProjectDetailDto
{
    public int Id { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string PartNumber { get; init; } = string.Empty;
    public string? ModelCode { get; init; }
    public ProjectStatus Status { get; init; }
    public APQPPhase CurrentPhase { get; init; }
    public DateTime? SOPDate { get; init; }
    public string? Description { get; init; }
    public string ProjectManagerId { get; init; } = string.Empty;
    public string? ProjectManagerName { get; init; }
    public decimal OverallProgress { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    public List<PhaseDto> Phases { get; init; } = new();
    public List<GateReviewDto> GateReviews { get; init; } = new();
    public List<PPAPDocumentDto> PPAPDocuments { get; init; } = new();
    public List<MilestoneDto> Milestones { get; init; } = new();
    public List<IssueDto> Issues { get; init; } = new();
    public List<EquipmentDto> Equipment { get; init; } = new();

    public int OpenIssueCount { get; init; }
    public int PPAPCompletedCount { get; init; }
    public int PPAPTotalCount { get; init; }
}

public record PhaseDto
{
    public int Id { get; init; }
    public APQPPhase Phase { get; init; }
    public PhaseStatus Status { get; init; }
    public DateTime? PlannedStartDate { get; init; }
    public DateTime? PlannedEndDate { get; init; }
    public DateTime? ActualStartDate { get; init; }
    public DateTime? ActualEndDate { get; init; }
    public decimal CompletionRate { get; init; }
    public int DeliverableCount { get; init; }
    public int CompletedDeliverableCount { get; init; }
}

public record GateReviewDto
{
    public int Id { get; init; }
    public int GateNumber { get; init; }
    public DateTime? ReviewDate { get; init; }
    public GateResult Result { get; init; }
    public string? ReviewerName { get; init; }
    public string? Notes { get; init; }
}

public record PPAPDocumentDto
{
    public int Id { get; init; }
    public int ElementNumber { get; init; }
    public string ElementName { get; init; } = string.Empty;
    public DocumentStatus Status { get; init; }
    public bool Required { get; init; }
    public string? FileName { get; init; }
    public DateTime? ApprovedDate { get; init; }
}

public record MilestoneDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public DateTime? ActualDate { get; init; }
    public MilestoneStatus Status { get; init; }
    public APQPPhase Phase { get; init; }
}

public record IssueDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public IssuePriority Priority { get; init; }
    public IssueStatus Status { get; init; }
    public IssueCategory Category { get; init; }
    public string? AssigneeName { get; init; }
    public DateTime? DueDate { get; init; }
}

public record EquipmentDto
{
    public int Id { get; init; }
    public string EquipmentCode { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public EquipmentType Type { get; init; }
    public EquipmentStatus Status { get; init; }
    public DateTime? DeliveryDate { get; init; }
}
