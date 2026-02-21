using AutoPartsPM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Features.Customers.Queries.GetCustomerList;

public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, List<CustomerListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerListDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Customers
            .Include(c => c.Projects)
            .AsQueryable();

        if (!request.IncludeInactive)
            query = query.Where(c => c.IsActive == true);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(c =>
                c.CustomerCode.ToLower().Contains(term) ||
                c.CustomerName.ToLower().Contains(term));
        }

        return await query
            .OrderBy(c => c.CustomerName)
            .Select(c => new CustomerListDto
            {
                Id = c.Id,
                CustomerCode = c.CustomerCode,
                CustomerName = c.CustomerName,
                ContactPerson = c.ContactPerson,
                Email = c.Email,
                Phone = c.Phone,
                IsActive = c.IsActive,
                ProjectCount = c.Projects.Count()
            })
            .ToListAsync(cancellationToken);
    }
}
