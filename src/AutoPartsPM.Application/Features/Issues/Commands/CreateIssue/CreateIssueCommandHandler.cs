using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Entities;
using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Commands.CreateIssue;

public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateIssueCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = new Issue
        {
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = IssueStatus.Open,
            Category = request.Category,
            AssigneeId = request.AssigneeId,
            AssigneeName = request.AssigneeName,
            DueDate = request.DueDate,
            RelatedPhase = request.RelatedPhase,
            CreatedBy = _currentUser.UserId ?? "system",
            UpdatedBy = _currentUser.UserId ?? "system"
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(issue.Id);
    }
}
