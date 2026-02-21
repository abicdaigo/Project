using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Equipment.Commands.DeleteEquipment;

public class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = await _context.Equipment.FindAsync(new object[] { request.Id }, cancellationToken);

        if (equipment is null)
            return Result.Failure("設備が見つかりません");

        _context.Equipment.Remove(equipment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
