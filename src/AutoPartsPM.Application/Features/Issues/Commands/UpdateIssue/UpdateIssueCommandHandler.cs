using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Commands.UpdateIssue;

public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateIssueCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await _context.Issues.FindAsync(new object[] { request.Id }, cancellationToken);

        if (issue is null)
            return Result.Failure("課題が見つかりません");

        issue.ProjectId = request.ProjectId;
        issue.Title = request.Title;
        issue.Description = request.Description;
        issue.Priority = request.Priority;
        issue.Status = request.Status;
        issue.Category = request.Category;
        issue.AssigneeId = request.AssigneeId;
        issue.AssigneeName = request.AssigneeName;
        issue.DueDate = request.DueDate;
        issue.ResolvedDate = request.Status == IssueStatus.Resolved && issue.ResolvedDate == null
            ? DateTime.UtcNow
            : request.ResolvedDate;
        issue.Resolution = request.Resolution;
        issue.RootCause = request.RootCause;
        issue.RelatedPhase = request.RelatedPhase;
        issue.UpdatedAt = DateTime.UtcNow;
        issue.UpdatedBy = _currentUser.UserId ?? "system";

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
