using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Commands.CreateDeliverable;

public record CreateDeliverableCommand : IRequest<Result<int>>
{
    public int ProjectPhaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Required { get; set; } = true;
}
