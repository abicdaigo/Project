using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.GateReviews.Commands.UpdateGateReview;

public record UpdateGateReviewCommand : IRequest<Result>
{
    public int Id { get; init; }
    public DateTime? ReviewDate { get; init; }
    public GateResult Result { get; init; }
    public string? ReviewerId { get; init; }
    public string? ReviewerName { get; init; }
    public string? Notes { get; init; }
    public string? ActionItems { get; init; }
}
