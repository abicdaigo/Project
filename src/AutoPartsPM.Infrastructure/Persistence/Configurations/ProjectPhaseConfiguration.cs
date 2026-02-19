using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoPartsPM.Infrastructure.Persistence.Configurations;

public class ProjectPhaseConfiguration : IEntityTypeConfiguration<ProjectPhase>
{
    public void Configure(EntityTypeBuilder<ProjectPhase> builder)
    {
        builder.ToTable("ProjectPhases");
        builder.HasKey(pp => pp.Id);

        builder.Property(pp => pp.CompletionRate).HasPrecision(5, 2);
        builder.Property(pp => pp.Notes).HasMaxLength(2000);

        builder.HasIndex(pp => new { pp.ProjectId, pp.Phase }).IsUnique();

        builder.HasOne(pp => pp.Project)
            .WithMany(p => p.Phases)
            .HasForeignKey(pp => pp.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
