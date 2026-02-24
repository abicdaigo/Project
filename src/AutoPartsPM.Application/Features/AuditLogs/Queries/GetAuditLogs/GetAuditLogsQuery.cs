using MediatR;

namespace AutoPartsPM.Application.Features.AuditLogs.Queries.GetAuditLogs;

public record GetAuditLogsQuery : IRequest<List<AuditLogDto>>
{
    public string? EntityName { get; set; }
    public string? Action { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int PageSize { get; set; } = 50;
}

public record AuditLogDto
{
    public int Id { get; init; }
    public string EntityName { get; init; } = string.Empty;
    public int EntityId { get; init; }
    public string Action { get; init; } = string.Empty;
    public string? UserName { get; init; }
    public DateTime Timestamp { get; init; }
    public string? Changes { get; init; }
}
