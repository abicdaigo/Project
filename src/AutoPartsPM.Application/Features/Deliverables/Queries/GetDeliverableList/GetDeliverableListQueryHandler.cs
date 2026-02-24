using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Deliverables.Queries.GetDeliverableList;

public class GetDeliverableListQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetDeliverableListQuery, List<DeliverableListDto>>
{
    public async Task<List<DeliverableListDto>> Handle(GetDeliverableListQuery request, CancellationToken cancellationToken)
    {
        return await context.Deliverables
            .Where(d => d.ProjectPhaseId == request.ProjectPhaseId)
            .OrderBy(d => d.DueDate)
            .Select(d => new DeliverableListDto
            {
                Id = d.Id,
                ProjectPhaseId = d.ProjectPhaseId,
                Name = d.Name,
                Description = d.Description,
                Status = d.Status,
                DueDate = d.DueDate,
                Required = d.Required,
                CompletedDate = d.CompletedDate
            })
            .ToListAsync(cancellationToken);
    }
}
