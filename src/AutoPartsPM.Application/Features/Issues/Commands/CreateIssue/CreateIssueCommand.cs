using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Commands.CreateIssue;

public record CreateIssueCommand : IRequest<Result<int>>
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IssuePriority Priority { get; set; } = IssuePriority.Medium;
    public IssueCategory Category { get; set; } = IssueCategory.Other;
    public string? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }
    public DateTime? DueDate { get; set; }
    public APQPPhase? RelatedPhase { get; set; }
}
