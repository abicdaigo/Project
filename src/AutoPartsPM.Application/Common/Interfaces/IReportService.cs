namespace AutoPartsPM.Application.Common.Interfaces;

public interface IReportService
{
    Task<byte[]> GenerateProjectReportAsync(int projectId, CancellationToken cancellationToken = default);
}
