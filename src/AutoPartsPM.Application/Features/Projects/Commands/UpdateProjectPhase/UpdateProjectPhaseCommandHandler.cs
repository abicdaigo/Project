using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProjectPhase;

public class UpdateProjectPhaseCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateProjectPhaseCommand, Result<int>>
{
    public async Task<Result<int>> Handle(UpdateProjectPhaseCommand request, CancellationToken cancellationToken)
    {
        var phase = await context.ProjectPhases.FindAsync([request.Id], cancellationToken);
        if (phase is null)
            return Result<int>.Failure("フェーズが見つかりません");

        phase.CompletionRate = request.Progress;
        phase.ActualStartDate = request.ActualStartDate;
        phase.Notes = request.Notes;

        // Progress 100% で ActualEndDate 未設定なら今日を設定
        if (request.Progress >= 100 && !request.ActualEndDate.HasValue)
            phase.ActualEndDate = DateTime.Today;
        else
            phase.ActualEndDate = request.ActualEndDate;

        // ステータス更新
        phase.Status = request.Progress switch
        {
            0 => PhaseStatus.NotStarted,
            100 => PhaseStatus.Completed,
            _ => PhaseStatus.InProgress
        };

        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(phase.Id);
    }
}
