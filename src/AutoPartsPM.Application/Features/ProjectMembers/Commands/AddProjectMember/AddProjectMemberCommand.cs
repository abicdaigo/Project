using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.ProjectMembers.Commands.AddProjectMember;

public record AddProjectMemberCommand : IRequest<Result<int>>
{
    public int ProjectId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Department { get; set; }
}
