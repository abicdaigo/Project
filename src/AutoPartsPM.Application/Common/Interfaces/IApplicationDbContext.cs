using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Project> Projects { get; }
    DbSet<ProjectPhase> ProjectPhases { get; }
    DbSet<GateReview> GateReviews { get; }
    DbSet<PPAPDocument> PPAPDocuments { get; }
    DbSet<Milestone> Milestones { get; }
    DbSet<Issue> Issues { get; }
    DbSet<Deliverable> Deliverables { get; }
    DbSet<Equipment> Equipment { get; }
    DbSet<ProjectMember> ProjectMembers { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
