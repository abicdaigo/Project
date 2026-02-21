using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(new object[] { request.Id }, cancellationToken);

        if (customer == null)
            return Result.Failure(new[] { "顧客が見つかりません" });

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
