using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Customers.Commands.DeleteCustomer;

public record DeleteCustomerCommand(int Id) : IRequest<Result>;
