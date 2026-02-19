using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand : IRequest<Result<int>>
{
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int CustomerId { get; init; }
    public string PartNumber { get; init; } = string.Empty;
    public string? ModelCode { get; init; }
    public DateTime? SOPDate { get; init; }
    public string? Description { get; init; }
    public string ProjectManagerId { get; init; } = string.Empty;
    public string? ProjectManagerName { get; init; }
}
