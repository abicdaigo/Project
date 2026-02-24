using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProjectPhase;

public record UpdateProjectPhaseCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public decimal Progress { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string? Notes { get; set; }
}
