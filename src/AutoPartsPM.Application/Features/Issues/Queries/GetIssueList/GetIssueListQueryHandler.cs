using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Issues.Queries.GetIssueList;

public class GetIssueListQueryHandler : IRequestHandler<GetIssueListQuery, List<IssueListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetIssueListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<IssueListDto>> Handle(GetIssueListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Issues
            .Include(i => i.Project)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            query = query.Where(i => i.ProjectId == request.ProjectId.Value);

        if (request.Status.HasValue)
            query = query.Where(i => i.Status == request.Status.Value);

        if (request.Priority.HasValue)
            query = query.Where(i => i.Priority == request.Priority.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(i => i.Title.ToLower().Contains(term));
        }

        return await query
            .OrderByDescending(i => i.Priority)
            .ThenByDescending(i => i.UpdatedAt)
            .Select(i => new IssueListDto
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                ProjectCode = i.Project.ProjectCode,
                ProjectName = i.Project.ProjectName,
                Title = i.Title,
                Priority = i.Priority,
                Status = i.Status,
                Category = i.Category,
                AssigneeName = i.AssigneeName,
                DueDate = i.DueDate,
                RelatedPhase = i.RelatedPhase,
                UpdatedAt = i.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
