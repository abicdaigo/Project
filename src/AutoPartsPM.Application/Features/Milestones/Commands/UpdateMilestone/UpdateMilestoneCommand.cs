using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Milestones.Commands.UpdateMilestone;

public record UpdateMilestoneCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? ActualDate { get; init; }
    public MilestoneStatus Status { get; init; }
    public APQPPhase Phase { get; init; }
    public int SortOrder { get; init; }
}
