using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Dashboard.Queries.GetDashboardData;

public class GetDashboardDataQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetDashboardDataQuery, DashboardDataDto>
{
    public async Task<DashboardDataDto> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var in7Days = today.AddDays(7);

        var projects = await context.Projects.ToListAsync(cancellationToken);

        var phaseDistribution = projects
            .Where(p => p.Status == ProjectStatus.Active)
            .GroupBy(p => p.CurrentPhase)
            .ToDictionary(g => g.Key, g => g.Count());

        var statusDistribution = projects
            .GroupBy(p => p.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        var overdueIssues = await context.Issues
            .Include(i => i.Project)
            .Where(i => i.DueDate < today
                && (i.Status == IssueStatus.Open || i.Status == IssueStatus.InProgress))
            .OrderBy(i => i.DueDate)
            .Take(10)
            .Select(i => new OverdueIssueDto
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                ProjectName = i.Project.ProjectName,
                Title = i.Title,
                Priority = i.Priority,
                DueDate = i.DueDate
            })
            .ToListAsync(cancellationToken);

        var upcomingMilestones = await context.Milestones
            .Include(m => m.Project)
            .Where(m => m.DueDate >= today && m.DueDate <= in7Days
                && m.Status != MilestoneStatus.Completed)
            .OrderBy(m => m.DueDate)
            .Take(10)
            .Select(m => new UpcomingMilestoneDto
            {
                Id = m.Id,
                ProjectId = m.ProjectId,
                ProjectName = m.Project.ProjectName,
                Name = m.Name,
                DueDate = m.DueDate,
                Status = m.Status
            })
            .ToListAsync(cancellationToken);

        return new DashboardDataDto
        {
            TotalProjects = projects.Count,
            ActiveProjects = projects.Count(p => p.Status == ProjectStatus.Active),
            CompletedProjects = projects.Count(p => p.Status == ProjectStatus.Completed),
            OverdueIssues = overdueIssues.Count,
            UpcomingMilestones = upcomingMilestones.Count,
            PhaseDistribution = phaseDistribution,
            StatusDistribution = statusDistribution,
            UpcomingMilestoneList = upcomingMilestones,
            OverdueIssueList = overdueIssues
        };
    }
}
