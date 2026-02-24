using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string? ModelCode { get; set; }
    public DateTime? SOPDate { get; set; }
    public string? Description { get; set; }
    public string? ProjectManagerName { get; set; }
    public ProjectStatus Status { get; set; }
}
