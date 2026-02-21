using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Customers.Queries.GetCustomerDetail;

public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, CustomerDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDetailDto?> Handle(GetCustomerDetailQuery request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (customer == null)
            return null;

        return new CustomerDetailDto
        {
            Id = customer.Id,
            CustomerCode = customer.CustomerCode,
            CustomerName = customer.CustomerName,
            ContactPerson = customer.ContactPerson,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            Projects = customer.Projects.Select(p => new CustomerProjectDto
            {
                Id = p.Id,
                ProjectCode = p.ProjectCode,
                ProjectName = p.ProjectName,
                Status = p.Status,
                CurrentPhase = p.CurrentPhase,
                SOPDate = p.SOPDate,
                ProjectManagerName = p.ProjectManagerName
            }).ToList()
        };
    }
}
