using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.GateReviews.Queries.GetGateReviewList;

public record GetGateReviewListQuery : IRequest<List<GateReviewListDto>>
{
    public int? ProjectId { get; init; }
    public GateResult? Result { get; init; }
}

public record GateReviewListDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int GateNumber { get; init; }
    public DateTime? ReviewDate { get; init; }
    public GateResult Result { get; init; }
    public string? ReviewerName { get; init; }
    public string? Notes { get; init; }
}
