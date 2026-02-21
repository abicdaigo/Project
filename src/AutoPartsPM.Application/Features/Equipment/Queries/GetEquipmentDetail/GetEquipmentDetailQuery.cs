using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Equipment.Queries.GetEquipmentDetail;

public record GetEquipmentDetailQuery(int Id) : IRequest<EquipmentDetailDto?>;

public record EquipmentDetailDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string EquipmentCode { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public EquipmentType Type { get; init; }
    public EquipmentStatus Status { get; init; }
    public string? Supplier { get; init; }
    public DateTime? OrderDate { get; init; }
    public DateTime? DeliveryDate { get; init; }
    public DateTime? ActualDeliveryDate { get; init; }
    public string? Location { get; init; }
    public string? Notes { get; init; }
    public decimal? Cost { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
}
