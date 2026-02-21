using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Equipment.Commands.CreateEquipment;

public class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateEquipmentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = new Domain.Entities.Equipment
        {
            ProjectId = request.ProjectId,
            EquipmentCode = request.EquipmentCode,
            Name = request.Name,
            Type = request.Type,
            Status = EquipmentStatus.Planning,
            Supplier = request.Supplier,
            OrderDate = request.OrderDate,
            DeliveryDate = request.DeliveryDate,
            Location = request.Location,
            Notes = request.Notes,
            Cost = request.Cost,
            CreatedBy = _currentUser.UserId ?? "system",
            UpdatedBy = _currentUser.UserId ?? "system"
        };

        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(equipment.Id);
    }
}
