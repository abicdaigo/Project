using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Entities;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Commands.CreateDeliverable;

public class CreateDeliverableCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateDeliverableCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateDeliverableCommand request, CancellationToken cancellationToken)
    {
        var deliverable = new Deliverable
        {
            ProjectPhaseId = request.ProjectPhaseId,
            Name = request.Name,
            Description = request.Description,
            DueDate = request.DueDate,
            Required = request.Required,
            Status = DeliverableStatus.NotStarted
        };

        context.Deliverables.Add(deliverable);
        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(deliverable.Id);
    }
}
