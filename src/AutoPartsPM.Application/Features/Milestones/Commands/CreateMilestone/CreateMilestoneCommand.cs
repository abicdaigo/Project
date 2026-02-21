using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Milestones.Commands.CreateMilestone;

public record CreateMilestoneCommand : IRequest<Result<int>>
{
    public int ProjectId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public APQPPhase Phase { get; init; }
    public int SortOrder { get; init; }
}
