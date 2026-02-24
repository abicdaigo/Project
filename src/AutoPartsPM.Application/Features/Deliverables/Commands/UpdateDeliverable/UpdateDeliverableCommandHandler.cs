using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Commands.UpdateDeliverable;

public class UpdateDeliverableCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateDeliverableCommand, Result<int>>
{
    public async Task<Result<int>> Handle(UpdateDeliverableCommand request, CancellationToken cancellationToken)
    {
        var deliverable = await context.Deliverables.FindAsync([request.Id], cancellationToken);
        if (deliverable is null)
            return Result<int>.Failure("成果物が見つかりません");

        deliverable.Name = request.Name;
        deliverable.Description = request.Description;
        deliverable.Status = request.Status;
        deliverable.DueDate = request.DueDate;

        if (request.Status == DeliverableStatus.Completed && !deliverable.CompletedDate.HasValue)
            deliverable.CompletedDate = DateTime.Today;

        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(deliverable.Id);
    }
}
