using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Milestones.Commands.DeleteMilestone;

public record DeleteMilestoneCommand : IRequest<Result>
{
    public int Id { get; init; }
}
