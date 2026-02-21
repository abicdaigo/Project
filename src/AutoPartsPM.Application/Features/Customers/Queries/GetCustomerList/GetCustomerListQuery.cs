using MediatR;

namespace AutoPartsPM.Application.Features.Customers.Queries.GetCustomerList;

public record GetCustomerListQuery : IRequest<List<CustomerListDto>>
{
    public string? SearchTerm { get; init; }
    public bool IncludeInactive { get; init; } = false;
}

public record CustomerListDto
{
    public int Id { get; init; }
    public string CustomerCode { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string? ContactPerson { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; }
    public int ProjectCount { get; init; }
}
