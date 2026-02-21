using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Equipment.Queries.GetEquipmentList;

public class GetEquipmentListQueryHandler : IRequestHandler<GetEquipmentListQuery, List<EquipmentListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEquipmentListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EquipmentListDto>> Handle(GetEquipmentListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Equipment
            .Include(e => e.Project)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            query = query.Where(e => e.ProjectId == request.ProjectId.Value);

        if (request.Status.HasValue)
            query = query.Where(e => e.Status == request.Status.Value);

        if (request.Type.HasValue)
            query = query.Where(e => e.Type == request.Type.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(e =>
                e.EquipmentCode.ToLower().Contains(term) ||
                e.Name.ToLower().Contains(term));
        }

        return await query
            .OrderByDescending(e => e.UpdatedAt)
            .Select(e => new EquipmentListDto
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ProjectCode = e.Project.ProjectCode,
                ProjectName = e.Project.ProjectName,
                EquipmentCode = e.EquipmentCode,
                Name = e.Name,
                Type = e.Type,
                Status = e.Status,
                Supplier = e.Supplier,
                DeliveryDate = e.DeliveryDate,
                Cost = e.Cost
            })
            .ToListAsync(cancellationToken);
    }
}
