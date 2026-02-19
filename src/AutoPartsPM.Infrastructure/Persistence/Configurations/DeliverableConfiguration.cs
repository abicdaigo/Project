using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoPartsPM.Infrastructure.Persistence.Configurations;

public class DeliverableConfiguration : IEntityTypeConfiguration<Deliverable>
{
    public void Configure(EntityTypeBuilder<Deliverable> builder)
    {
        builder.ToTable("Deliverables");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.FilePath).HasMaxLength(500);
        builder.Property(d => d.FileName).HasMaxLength(200);

        builder.HasOne(d => d.ProjectPhase)
            .WithMany(pp => pp.Deliverables)
            .HasForeignKey(d => d.ProjectPhaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
