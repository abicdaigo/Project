using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Documents.Queries.GetDocumentList;

public class GetDocumentListQueryHandler : IRequestHandler<GetDocumentListQuery, List<DocumentListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDocumentListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DocumentListDto>> Handle(GetDocumentListQuery request, CancellationToken cancellationToken)
    {
        // PPAPドキュメント一覧を取得
        var ppapQuery = _context.PPAPDocuments
            .Include(p => p.Project)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            ppapQuery = ppapQuery.Where(p => p.ProjectId == request.ProjectId.Value);

        var ppapDocs = await ppapQuery
            .Select(p => new DocumentListDto
            {
                Id = p.Id,
                DocumentType = "PPAP",
                ProjectId = p.ProjectId,
                ProjectCode = p.Project.ProjectCode,
                ProjectName = p.Project.ProjectName,
                Name = p.ElementName,
                FileName = p.FileName,
                Status = p.Status.ToString(),
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        // 成果物一覧を取得
        var deliverableQuery = _context.Deliverables
            .Include(d => d.ProjectPhase)
            .ThenInclude(ph => ph.Project)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            deliverableQuery = deliverableQuery.Where(d => d.ProjectPhase.ProjectId == request.ProjectId.Value);

        var deliverables = await deliverableQuery
            .Select(d => new DocumentListDto
            {
                Id = d.Id,
                DocumentType = "成果物",
                ProjectId = d.ProjectPhase.ProjectId,
                ProjectCode = d.ProjectPhase.Project.ProjectCode,
                ProjectName = d.ProjectPhase.Project.ProjectName,
                Name = d.Name,
                FileName = d.FileName,
                Status = d.Status.ToString(),
                UpdatedAt = d.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        // 統合して検索フィルター適用
        var combined = ppapDocs.Concat(deliverables);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            combined = combined.Where(d => d.Name.ToLower().Contains(term));
        }

        return combined
            .OrderByDescending(d => d.UpdatedAt)
            .ToList();
    }
}
