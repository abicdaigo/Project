using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Dashboard.Queries.GetDashboardData;

public record GetDashboardDataQuery : IRequest<DashboardDataDto>;

public record DashboardDataDto
{
    public int TotalProjects { get; init; }
    public int ActiveProjects { get; init; }
    public int CompletedProjects { get; init; }
    public int OverdueIssues { get; init; }
    public int UpcomingMilestones { get; init; }
    public Dictionary<APQPPhase, int> PhaseDistribution { get; init; } = new();
    public Dictionary<ProjectStatus, int> StatusDistribution { get; init; } = new();
    public List<UpcomingMilestoneDto> UpcomingMilestoneList { get; init; } = new();
    public List<OverdueIssueDto> OverdueIssueList { get; init; } = new();
}

public record UpcomingMilestoneDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public MilestoneStatus Status { get; init; }
}

public record OverdueIssueDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public IssuePriority Priority { get; init; }
    public DateTime? DueDate { get; init; }
}
