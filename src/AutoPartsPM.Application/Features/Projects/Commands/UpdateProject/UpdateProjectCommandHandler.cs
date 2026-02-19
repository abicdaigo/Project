using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project is null)
            return Result.Failure("プロジェクトが見つかりません");

        project.ProjectName = request.ProjectName;
        project.PartNumber = request.PartNumber;
        project.ModelCode = request.ModelCode;
        project.Status = request.Status;
        project.SOPDate = request.SOPDate;
        project.Description = request.Description;
        project.ProjectManagerId = request.ProjectManagerId;
        project.ProjectManagerName = request.ProjectManagerName;
        project.UpdatedAt = DateTime.UtcNow;
        project.UpdatedBy = _currentUser.UserId ?? "system";

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
