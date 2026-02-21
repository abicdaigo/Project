using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.GateReviews.Queries.GetGateReviewList;

public class GetGateReviewListQueryHandler : IRequestHandler<GetGateReviewListQuery, List<GateReviewListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetGateReviewListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GateReviewListDto>> Handle(GetGateReviewListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.GateReviews
            .Include(g => g.Project)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            query = query.Where(g => g.ProjectId == request.ProjectId.Value);

        if (request.Result.HasValue)
            query = query.Where(g => g.Result == request.Result.Value);

        return await query
            .OrderBy(g => g.ProjectId)
            .ThenBy(g => g.GateNumber)
            .Select(g => new GateReviewListDto
            {
                Id = g.Id,
                ProjectId = g.ProjectId,
                ProjectCode = g.Project.ProjectCode,
                ProjectName = g.Project.ProjectName,
                GateNumber = g.GateNumber,
                ReviewDate = g.ReviewDate,
                Result = g.Result,
                ReviewerName = g.ReviewerName,
                Notes = g.Notes
            })
            .ToListAsync(cancellationToken);
    }
}
