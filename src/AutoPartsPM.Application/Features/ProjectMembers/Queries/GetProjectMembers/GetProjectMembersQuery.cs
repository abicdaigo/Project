using MediatR;

namespace AutoPartsPM.Application.Features.ProjectMembers.Queries.GetProjectMembers;

public record GetProjectMembersQuery(int ProjectId) : IRequest<List<ProjectMemberDto>>;

public record ProjectMemberDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string? Department { get; init; }
}
