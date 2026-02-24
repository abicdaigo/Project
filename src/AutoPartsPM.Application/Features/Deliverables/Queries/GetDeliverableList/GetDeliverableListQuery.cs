using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Queries.GetDeliverableList;

public record GetDeliverableListQuery : IRequest<List<DeliverableListDto>>
{
    public int ProjectPhaseId { get; init; }
}

public record DeliverableListDto
{
    public int Id { get; init; }
    public int ProjectPhaseId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DeliverableStatus Status { get; init; }
    public DateTime? DueDate { get; init; }
    public bool Required { get; init; }
    public DateTime? CompletedDate { get; init; }
}
