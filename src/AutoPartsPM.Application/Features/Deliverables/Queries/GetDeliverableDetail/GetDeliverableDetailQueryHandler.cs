using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Deliverables.Queries.GetDeliverableDetail;

public class GetDeliverableDetailQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetDeliverableDetailQuery, DeliverableDetailDto?>
{
    public async Task<DeliverableDetailDto?> Handle(GetDeliverableDetailQuery request, CancellationToken cancellationToken)
    {
        var deliverable = await context.Deliverables
            .Include(d => d.ProjectPhase)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (deliverable is null) return null;

        return new DeliverableDetailDto
        {
            Id = deliverable.Id,
            ProjectPhaseId = deliverable.ProjectPhaseId,
            PhaseName = deliverable.ProjectPhase.Phase.ToString(),
            Name = deliverable.Name,
            Description = deliverable.Description,
            Status = deliverable.Status,
            DueDate = deliverable.DueDate,
            Required = deliverable.Required,
            CompletedDate = deliverable.CompletedDate,
            FilePath = deliverable.FilePath,
            FileName = deliverable.FileName
        };
    }
}
