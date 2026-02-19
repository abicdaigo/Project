using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Queries.GetProjectList;

public record GetProjectListQuery : IRequest<List<ProjectListDto>>
{
    public string? SearchTerm { get; init; }
    public ProjectStatus? Status { get; init; }
    public int? CustomerId { get; init; }
}

public record ProjectListDto
{
    public int Id { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string PartNumber { get; init; } = string.Empty;
    public string? ModelCode { get; init; }
    public ProjectStatus Status { get; init; }
    public APQPPhase CurrentPhase { get; init; }
    public DateTime? SOPDate { get; init; }
    public string? ProjectManagerName { get; init; }
    public decimal OverallProgress { get; init; }
}
