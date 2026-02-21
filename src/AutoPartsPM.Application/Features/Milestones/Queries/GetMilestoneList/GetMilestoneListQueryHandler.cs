using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Milestones.Queries.GetMilestoneList;

public class GetMilestoneListQueryHandler : IRequestHandler<GetMilestoneListQuery, List<MilestoneListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMilestoneListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MilestoneListDto>> Handle(GetMilestoneListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Milestones
            .Include(m => m.Project)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            query = query.Where(m => m.ProjectId == request.ProjectId.Value);

        if (request.Status.HasValue)
            query = query.Where(m => m.Status == request.Status.Value);

        if (request.Phase.HasValue)
            query = query.Where(m => m.Phase == request.Phase.Value);

        var now = DateTime.UtcNow;

        return await query
            .OrderBy(m => m.DueDate)
            .Select(m => new MilestoneListDto
            {
                Id = m.Id,
                ProjectId = m.ProjectId,
                ProjectCode = m.Project.ProjectCode,
                ProjectName = m.Project.ProjectName,
                Name = m.Name,
                Description = m.Description,
                DueDate = m.DueDate,
                ActualDate = m.ActualDate,
                Status = m.Status,
                Phase = m.Phase,
                SortOrder = m.SortOrder,
                IsOverdue = m.Status != MilestoneStatus.Completed && m.DueDate < now
            })
            .ToListAsync(cancellationToken);
    }
}
