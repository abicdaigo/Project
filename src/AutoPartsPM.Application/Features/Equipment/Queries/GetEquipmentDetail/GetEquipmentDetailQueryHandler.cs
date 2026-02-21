using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Equipment.Queries.GetEquipmentDetail;

public class GetEquipmentDetailQueryHandler : IRequestHandler<GetEquipmentDetailQuery, EquipmentDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetEquipmentDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EquipmentDetailDto?> Handle(GetEquipmentDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Equipment
            .Include(e => e.Project)
            .Where(e => e.Id == request.Id)
            .Select(e => new EquipmentDetailDto
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
                OrderDate = e.OrderDate,
                DeliveryDate = e.DeliveryDate,
                ActualDeliveryDate = e.ActualDeliveryDate,
                Location = e.Location,
                Notes = e.Notes,
                Cost = e.Cost,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                CreatedBy = e.CreatedBy
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
