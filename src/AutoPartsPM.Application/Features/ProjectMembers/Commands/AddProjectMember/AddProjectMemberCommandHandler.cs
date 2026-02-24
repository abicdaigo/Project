using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Entities;
using MediatR;

namespace AutoPartsPM.Application.Features.ProjectMembers.Commands.AddProjectMember;

public class AddProjectMemberCommandHandler(IApplicationDbContext context)
    : IRequestHandler<AddProjectMemberCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var member = new ProjectMember
        {
            ProjectId = request.ProjectId,
            UserId = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Role = request.Role,
            Department = request.Department
        };

        context.ProjectMembers.Add(member);
        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(member.Id);
    }
}
