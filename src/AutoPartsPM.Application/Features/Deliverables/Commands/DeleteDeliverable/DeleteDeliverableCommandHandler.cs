using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Commands.DeleteDeliverable;

public class DeleteDeliverableCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteDeliverableCommand, Result<int>>
{
    public async Task<Result<int>> Handle(DeleteDeliverableCommand request, CancellationToken cancellationToken)
    {
        var deliverable = await context.Deliverables.FindAsync([request.Id], cancellationToken);
        if (deliverable is null)
            return Result<int>.Failure("成果物が見つかりません");

        context.Deliverables.Remove(deliverable);
        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(request.Id);
    }
}
