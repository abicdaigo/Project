using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Queries.GetIssueList;

public record GetIssueListQuery : IRequest<List<IssueListDto>>
{
    public int? ProjectId { get; init; }
    public IssueStatus? Status { get; init; }
    public IssuePriority? Priority { get; init; }
    public string? SearchTerm { get; init; }
}

public record IssueListDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public IssuePriority Priority { get; init; }
    public IssueStatus Status { get; init; }
    public IssueCategory Category { get; init; }
    public string? AssigneeName { get; init; }
    public DateTime? DueDate { get; init; }
    public APQPPhase? RelatedPhase { get; init; }
    public DateTime UpdatedAt { get; init; }
}
