using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Milestones.Commands.UpdateMilestone;

public class UpdateMilestoneCommandHandler : IRequestHandler<UpdateMilestoneCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateMilestoneCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateMilestoneCommand request, CancellationToken cancellationToken)
    {
        var milestone = await _context.Milestones
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (milestone is null)
            return Result.Failure("マイルストーンが見つかりません");

        milestone.Name = request.Name;
        milestone.Description = request.Description;
        milestone.DueDate = request.DueDate;
        milestone.ActualDate = request.ActualDate;
        milestone.Status = request.Status;
        milestone.Phase = request.Phase;
        milestone.SortOrder = request.SortOrder;
        milestone.UpdatedAt = DateTime.UtcNow;
        milestone.UpdatedBy = _currentUser.UserId ?? "system";

        if (request.Status == MilestoneStatus.Completed && milestone.ActualDate == null)
        {
            milestone.ActualDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
