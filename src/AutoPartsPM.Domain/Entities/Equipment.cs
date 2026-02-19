using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class Equipment : BaseEntity
{
    public int ProjectId { get; set; }
    public string EquipmentCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public EquipmentType Type { get; set; }
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Planning;
    public string? Supplier { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public decimal? Cost { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
