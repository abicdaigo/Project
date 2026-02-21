using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.GateReviews.Commands.UpdateGateReview;

public class UpdateGateReviewCommandHandler : IRequestHandler<UpdateGateReviewCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateGateReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateGateReviewCommand request, CancellationToken cancellationToken)
    {
        var gateReview = await _context.GateReviews.FindAsync(new object[] { request.Id }, cancellationToken);

        if (gateReview is null)
            return Result.Failure("ゲートレビューが見つかりません");

        gateReview.ReviewDate = request.ReviewDate;
        gateReview.Result = request.Result;
        gateReview.ReviewerId = request.ReviewerId;
        gateReview.ReviewerName = request.ReviewerName;
        gateReview.Notes = request.Notes;
        gateReview.ActionItems = request.ActionItems;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
