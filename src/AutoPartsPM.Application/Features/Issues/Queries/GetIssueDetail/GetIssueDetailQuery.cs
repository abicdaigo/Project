using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Queries.GetIssueDetail;

public record GetIssueDetailQuery(int Id) : IRequest<IssueDetailDto?>;

public record IssueDetailDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public IssuePriority Priority { get; init; }
    public IssueStatus Status { get; init; }
    public IssueCategory Category { get; init; }
    public string? AssigneeId { get; init; }
    public string? AssigneeName { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? ResolvedDate { get; init; }
    public string? Resolution { get; init; }
    public string? RootCause { get; init; }
    public APQPPhase? RelatedPhase { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
}
