using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.ProjectMembers.Commands.RemoveProjectMember;

public record RemoveProjectMemberCommand(int Id) : IRequest<Result<int>>;
