using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand : IRequest<Result<int>>
{
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string? ModelCode { get; set; }
    public DateTime? SOPDate { get; set; }
    public string? Description { get; set; }
    public string ProjectManagerId { get; set; } = string.Empty;
    public string? ProjectManagerName { get; set; }
}
