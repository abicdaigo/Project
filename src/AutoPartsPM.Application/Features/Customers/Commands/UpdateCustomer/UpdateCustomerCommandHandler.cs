using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateCustomerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(new object[] { request.Id }, cancellationToken);

        if (customer == null)
            return Result.Failure(new[] { "顧客が見つかりません" });

        customer.CustomerCode = request.CustomerCode;
        customer.CustomerName = request.CustomerName;
        customer.ContactPerson = request.ContactPerson;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Address = request.Address;
        customer.IsActive = request.IsActive;
        customer.UpdatedAt = DateTime.UtcNow;
        customer.UpdatedBy = _currentUser.UserId ?? "system";

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
