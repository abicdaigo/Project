using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.PPAP.Queries.GetPPAPDocumentList;

public class GetPPAPDocumentListQueryHandler : IRequestHandler<GetPPAPDocumentListQuery, List<PPAPDocumentListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPPAPDocumentListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PPAPDocumentListDto>> Handle(GetPPAPDocumentListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PPAPDocuments
            .Include(p => p.Project)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            query = query.Where(p => p.ProjectId == request.ProjectId.Value);

        if (request.Status.HasValue)
            query = query.Where(p => p.Status == request.Status.Value);

        return await query
            .OrderBy(p => p.ProjectId)
            .ThenBy(p => p.ElementNumber)
            .Select(p => new PPAPDocumentListDto
            {
                Id = p.Id,
                ProjectId = p.ProjectId,
                ProjectCode = p.Project.ProjectCode,
                ProjectName = p.Project.ProjectName,
                ElementNumber = p.ElementNumber,
                ElementName = p.ElementName,
                Status = p.Status,
                SubmissionLevel = p.SubmissionLevel,
                Required = p.Required,
                ApprovedDate = p.ApprovedDate,
                ApprovedBy = p.ApprovedBy,
                Notes = p.Notes
            })
            .ToListAsync(cancellationToken);
    }
}
