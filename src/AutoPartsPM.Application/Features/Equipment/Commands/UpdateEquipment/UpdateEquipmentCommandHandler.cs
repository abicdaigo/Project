using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Equipment.Commands.UpdateEquipment;

public class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateEquipmentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = await _context.Equipment.FindAsync(new object[] { request.Id }, cancellationToken);

        if (equipment is null)
            return Result.Failure("設備が見つかりません");

        equipment.ProjectId = request.ProjectId;
        equipment.EquipmentCode = request.EquipmentCode;
        equipment.Name = request.Name;
        equipment.Type = request.Type;
        equipment.Status = request.Status;
        equipment.Supplier = request.Supplier;
        equipment.OrderDate = request.OrderDate;
        equipment.DeliveryDate = request.DeliveryDate;
        equipment.ActualDeliveryDate = request.ActualDeliveryDate;
        equipment.Location = request.Location;
        equipment.Notes = request.Notes;
        equipment.Cost = request.Cost;
        equipment.UpdatedAt = DateTime.UtcNow;
        equipment.UpdatedBy = _currentUser.UserId ?? "system";

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
