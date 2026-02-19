using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Entities;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            ProjectCode = request.ProjectCode,
            ProjectName = request.ProjectName,
            CustomerId = request.CustomerId,
            PartNumber = request.PartNumber,
            ModelCode = request.ModelCode,
            Status = ProjectStatus.Draft,
            CurrentPhase = APQPPhase.Planning,
            SOPDate = request.SOPDate,
            Description = request.Description,
            ProjectManagerId = request.ProjectManagerId,
            ProjectManagerName = request.ProjectManagerName,
            CreatedBy = _currentUser.UserId ?? "system",
            UpdatedBy = _currentUser.UserId ?? "system"
        };

        // APQPの5フェーズを自動作成
        foreach (APQPPhase phase in Enum.GetValues<APQPPhase>())
        {
            project.Phases.Add(new ProjectPhase
            {
                Phase = phase,
                Status = phase == APQPPhase.Planning ? PhaseStatus.InProgress : PhaseStatus.NotStarted,
                CreatedBy = _currentUser.UserId ?? "system",
                UpdatedBy = _currentUser.UserId ?? "system"
            });
        }

        // PPAP 18書類を自動作成
        for (int i = 1; i <= 18; i++)
        {
            project.PPAPDocuments.Add(new PPAPDocument
            {
                ElementNumber = i,
                ElementName = PPAPDocument.GetElementName(i),
                Status = DocumentStatus.NotStarted,
                SubmissionLevel = PPAPLevel.Level3,
                Required = true,
                CreatedBy = _currentUser.UserId ?? "system",
                UpdatedBy = _currentUser.UserId ?? "system"
            });
        }

        // ゲートレビュー（5ゲート）を自動作成
        for (int i = 1; i <= 5; i++)
        {
            project.GateReviews.Add(new GateReview
            {
                GateNumber = i,
                Result = GateResult.NotReviewed,
                CreatedBy = _currentUser.UserId ?? "system",
                UpdatedBy = _currentUser.UserId ?? "system"
            });
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(project.Id);
    }
}
