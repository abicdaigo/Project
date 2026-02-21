using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Issues.Commands.DeleteIssue;

public record DeleteIssueCommand(int Id) : IRequest<Result>;
