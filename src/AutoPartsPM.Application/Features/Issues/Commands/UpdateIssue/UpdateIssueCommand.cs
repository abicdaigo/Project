using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Commands.UpdateIssue;

public record UpdateIssueCommand : IRequest<Result>
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
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
}
