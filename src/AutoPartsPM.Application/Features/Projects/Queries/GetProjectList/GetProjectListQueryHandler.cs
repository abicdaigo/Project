using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Projects.Queries.GetProjectList;

public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery, List<ProjectListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectListDto>> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Projects
            .Include(p => p.Customer)
            .Include(p => p.Phases)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(p =>
                p.ProjectCode.ToLower().Contains(term) ||
                p.ProjectName.ToLower().Contains(term) ||
                p.PartNumber.ToLower().Contains(term) ||
                p.Customer.CustomerName.ToLower().Contains(term));
        }

        if (request.Status.HasValue)
            query = query.Where(p => p.Status == request.Status.Value);

        if (request.CustomerId.HasValue)
            query = query.Where(p => p.CustomerId == request.CustomerId.Value);

        return await query
            .OrderByDescending(p => p.UpdatedAt)
            .Select(p => new ProjectListDto
            {
                Id = p.Id,
                ProjectCode = p.ProjectCode,
                ProjectName = p.ProjectName,
                CustomerName = p.Customer.CustomerName,
                PartNumber = p.PartNumber,
                ModelCode = p.ModelCode,
                Status = p.Status,
                CurrentPhase = p.CurrentPhase,
                SOPDate = p.SOPDate,
                ProjectManagerName = p.ProjectManagerName,
                OverallProgress = p.Phases.Any()
                    ? p.Phases.Average(ph => ph.CompletionRate)
                    : 0
            })
            .ToListAsync(cancellationToken);
    }
}
