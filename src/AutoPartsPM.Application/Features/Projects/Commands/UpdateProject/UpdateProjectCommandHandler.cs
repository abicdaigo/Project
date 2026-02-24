using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateProjectCommand, Result<int>>
{
    public async Task<Result<int>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await context.Projects.FindAsync([request.Id], cancellationToken);
        if (project is null)
            return Result<int>.Failure("プロジェクトが見つかりません");

        // 重複チェック（自身除外）
        var duplicateCode = await context.Projects
            .AnyAsync(p => p.ProjectCode == request.ProjectCode && p.Id != request.Id, cancellationToken);
        if (duplicateCode)
            return Result<int>.Failure("このプロジェクトコードは既に使用されています");

        project.ProjectCode = request.ProjectCode;
        project.ProjectName = request.ProjectName;
        project.PartNumber = request.PartNumber;
        project.ModelCode = request.ModelCode;
        project.SOPDate = request.SOPDate;
        project.Description = request.Description;
        project.ProjectManagerName = request.ProjectManagerName;
        project.Status = request.Status;

        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(project.Id);
    }
}
