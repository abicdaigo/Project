using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using AutoPartsPM.Domain.Entities;
using MediatR;

namespace AutoPartsPM.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateCustomerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            ContactPerson = request.ContactPerson,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            IsActive = request.IsActive,
            CreatedBy = _currentUser.UserId ?? "system",
            UpdatedBy = _currentUser.UserId ?? "system"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(customer.Id);
    }
}
