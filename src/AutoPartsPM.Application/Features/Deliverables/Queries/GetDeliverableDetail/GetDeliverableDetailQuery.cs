using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Queries.GetDeliverableDetail;

public record GetDeliverableDetailQuery(int Id) : IRequest<DeliverableDetailDto?>;

public record DeliverableDetailDto
{
    public int Id { get; init; }
    public int ProjectPhaseId { get; init; }
    public string PhaseName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DeliverableStatus Status { get; init; }
    public DateTime? DueDate { get; init; }
    public bool Required { get; init; }
    public DateTime? CompletedDate { get; init; }
    public string? FilePath { get; init; }
    public string? FileName { get; init; }
}
