using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Equipment.Queries.GetEquipmentList;

public record GetEquipmentListQuery : IRequest<List<EquipmentListDto>>
{
    public int? ProjectId { get; init; }
    public EquipmentStatus? Status { get; init; }
    public EquipmentType? Type { get; init; }
    public string? SearchTerm { get; init; }
}

public record EquipmentListDto
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
    public DateTime? DeliveryDate { get; init; }
    public decimal? Cost { get; init; }
}
