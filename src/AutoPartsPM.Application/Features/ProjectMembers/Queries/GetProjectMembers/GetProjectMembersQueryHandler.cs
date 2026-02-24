using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.ProjectMembers.Queries.GetProjectMembers;

public class GetProjectMembersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetProjectMembersQuery, List<ProjectMemberDto>>
{
    public async Task<List<ProjectMemberDto>> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
    {
        return await context.ProjectMembers
            .Where(m => m.ProjectId == request.ProjectId)
            .OrderBy(m => m.UserName)
            .Select(m => new ProjectMemberDto
            {
                Id = m.Id,
                ProjectId = m.ProjectId,
                UserName = m.UserName,
                Role = m.Role,
                Department = m.Department
            })
            .ToListAsync(cancellationToken);
    }
}
