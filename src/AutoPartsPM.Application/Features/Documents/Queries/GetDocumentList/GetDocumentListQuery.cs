using MediatR;

namespace AutoPartsPM.Application.Features.Documents.Queries.GetDocumentList;

public record GetDocumentListQuery : IRequest<List<DocumentListDto>>
{
    public int? ProjectId { get; init; }
    public string? SearchTerm { get; init; }
}

public record DocumentListDto
{
    public int Id { get; init; }
    public string DocumentType { get; init; } = string.Empty;
    public int ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? FileName { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime? UpdatedAt { get; init; }
}
