using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.ProjectMembers.Commands.RemoveProjectMember;

public class RemoveProjectMemberCommandHandler(IApplicationDbContext context)
    : IRequestHandler<RemoveProjectMemberCommand, Result<int>>
{
    public async Task<Result<int>> Handle(RemoveProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await context.ProjectMembers.FindAsync([request.Id], cancellationToken);
        if (member is null)
            return Result<int>.Failure("メンバーが見つかりません");

        context.ProjectMembers.Remove(member);
        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(request.Id);
    }
}
