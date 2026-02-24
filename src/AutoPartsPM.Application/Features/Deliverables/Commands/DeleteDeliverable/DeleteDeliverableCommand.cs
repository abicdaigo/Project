using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Deliverables.Commands.DeleteDeliverable;

public record DeleteDeliverableCommand(int Id) : IRequest<Result<int>>;
