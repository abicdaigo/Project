using AutoPartsPM.Domain.Entities;
using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetProjectWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Project>> GetProjectsByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Project>> GetProjectsByStatusAsync(ProjectStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Project>> GetProjectsByManagerAsync(string managerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Project>> SearchProjectsAsync(string? searchTerm, ProjectStatus? status, int? customerId, CancellationToken cancellationToken = default);
}
