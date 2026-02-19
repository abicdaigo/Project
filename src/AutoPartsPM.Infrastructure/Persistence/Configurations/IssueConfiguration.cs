using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoPartsPM.Infrastructure.Persistence.Configurations;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("Issues");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Title).IsRequired().HasMaxLength(500);
        builder.Property(i => i.Description).HasMaxLength(4000);
        builder.Property(i => i.AssigneeId).HasMaxLength(200);
        builder.Property(i => i.AssigneeName).HasMaxLength(200);
        builder.Property(i => i.Resolution).HasMaxLength(4000);
        builder.Property(i => i.RootCause).HasMaxLength(4000);

        builder.HasIndex(i => i.Status);
        builder.HasIndex(i => i.Priority);

        builder.HasOne(i => i.Project)
            .WithMany(p => p.Issues)
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
