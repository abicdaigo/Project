using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.PPAP.Queries.GetPPAPDocumentList;

public record GetPPAPDocumentListQuery : IRequest<List<PPAPDocumentListDto>>
{
    public int? ProjectId { get; init; }
    public DocumentStatus? Status { get; init; }
}

public record PPAPDocumentListDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int ElementNumber { get; init; }
    public string ElementName { get; init; } = string.Empty;
    public DocumentStatus Status { get; init; }
    public PPAPLevel SubmissionLevel { get; init; }
    public bool Required { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public string? ApprovedBy { get; init; }
    public string? Notes { get; init; }
}
