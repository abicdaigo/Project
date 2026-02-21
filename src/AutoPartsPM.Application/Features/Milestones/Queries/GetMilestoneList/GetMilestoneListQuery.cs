using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Milestones.Queries.GetMilestoneList;

public record GetMilestoneListQuery : IRequest<List<MilestoneListDto>>
{
    public int? ProjectId { get; init; }
    public MilestoneStatus? Status { get; init; }
    public APQPPhase? Phase { get; init; }
}

public record MilestoneListDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? ActualDate { get; init; }
    public MilestoneStatus Status { get; init; }
    public APQPPhase Phase { get; init; }
    public int SortOrder { get; init; }
    public bool IsOverdue { get; init; }
}
