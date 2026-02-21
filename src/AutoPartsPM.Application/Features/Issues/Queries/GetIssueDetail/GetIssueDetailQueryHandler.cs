using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Issues.Queries.GetIssueDetail;

public class GetIssueDetailQueryHandler : IRequestHandler<GetIssueDetailQuery, IssueDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetIssueDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IssueDetailDto?> Handle(GetIssueDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Issues
            .Include(i => i.Project)
            .Where(i => i.Id == request.Id)
            .Select(i => new IssueDetailDto
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                ProjectCode = i.Project.ProjectCode,
                ProjectName = i.Project.ProjectName,
                Title = i.Title,
                Description = i.Description,
                Priority = i.Priority,
                Status = i.Status,
                Category = i.Category,
                AssigneeId = i.AssigneeId,
                AssigneeName = i.AssigneeName,
                DueDate = i.DueDate,
                ResolvedDate = i.ResolvedDate,
                Resolution = i.Resolution,
                RootCause = i.RootCause,
                RelatedPhase = i.RelatedPhase,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
                CreatedBy = i.CreatedBy
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
