using AutoPartsPM.Domain.Enums;
using MediatR;

namespace AutoPartsPM.Application.Features.Customers.Queries.GetCustomerDetail;

public record GetCustomerDetailQuery(int Id) : IRequest<CustomerDetailDto?>;

public record CustomerDetailDto
{
    public int Id { get; init; }
    public string CustomerCode { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string? ContactPerson { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public List<CustomerProjectDto> Projects { get; init; } = new();
}

public record CustomerProjectDto
{
    public int Id { get; init; }
    public string ProjectCode { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public ProjectStatus Status { get; init; }
    public APQPPhase CurrentPhase { get; init; }
    public DateTime? SOPDate { get; init; }
    public string? ProjectManagerName { get; init; }
}
