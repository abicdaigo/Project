using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Milestones.Commands.DeleteMilestone;

public class DeleteMilestoneCommandHandler : IRequestHandler<DeleteMilestoneCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteMilestoneCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteMilestoneCommand request, CancellationToken cancellationToken)
    {
        var milestone = await _context.Milestones
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (milestone is null)
            return Result.Failure("マイルストーンが見つかりません");

        _context.Milestones.Remove(milestone);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
