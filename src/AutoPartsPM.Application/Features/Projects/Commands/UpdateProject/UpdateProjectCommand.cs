using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public string PartNumber { get; init; } = string.Empty;
    public string? ModelCode { get; init; }
    public ProjectStatus Status { get; init; }
    public DateTime? SOPDate { get; init; }
    public string? Description { get; init; }
    public string ProjectManagerId { get; init; } = string.Empty;
    public string? ProjectManagerName { get; init; }
}
