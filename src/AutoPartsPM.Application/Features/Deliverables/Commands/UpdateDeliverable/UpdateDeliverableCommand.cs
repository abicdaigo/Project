using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Commands.UpdateDeliverable;

public record UpdateDeliverableCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DeliverableStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
}
