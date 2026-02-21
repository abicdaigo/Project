using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Equipment.Commands.CreateEquipment;

public record CreateEquipmentCommand : IRequest<Result<int>>
{
    public int ProjectId { get; set; }
    public string EquipmentCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public EquipmentType Type { get; set; }
    public string? Supplier { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public decimal? Cost { get; set; }
}
