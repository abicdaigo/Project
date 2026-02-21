using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Entities;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Milestones.Commands.CreateMilestone;

public class CreateMilestoneCommandHandler : IRequestHandler<CreateMilestoneCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateMilestoneCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(CreateMilestoneCommand request, CancellationToken cancellationToken)
    {
        var milestone = new Milestone
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description,
            DueDate = request.DueDate,
            Phase = request.Phase,
            SortOrder = request.SortOrder,
            Status = MilestoneStatus.Pending,
            CreatedBy = _currentUser.UserId ?? "system",
            UpdatedBy = _currentUser.UserId ?? "system"
        };

        _context.Milestones.Add(milestone);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(milestone.Id);
    }
}
