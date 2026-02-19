using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectPhase> ProjectPhases => Set<ProjectPhase>();
    public DbSet<GateReview> GateReviews => Set<GateReview>();
    public DbSet<PPAPDocument> PPAPDocuments => Set<PPAPDocument>();
    public DbSet<Milestone> Milestones => Set<Milestone>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<Deliverable> Deliverables => Set<Deliverable>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService.UserId ?? "system";
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = _currentUserService.UserId ?? "system";
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = _currentUserService.UserId ?? "system";
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
