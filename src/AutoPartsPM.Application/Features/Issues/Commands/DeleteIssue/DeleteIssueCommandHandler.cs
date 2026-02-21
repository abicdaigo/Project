using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Commands.DeleteIssue;

public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteIssueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await _context.Issues.FindAsync(new object[] { request.Id }, cancellationToken);

        if (issue is null)
            return Result.Failure("課題が見つかりません");

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
