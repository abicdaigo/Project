using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoPartsPM.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProjectCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ProjectName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.PartNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ModelCode).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.ProjectManagerId).IsRequired().HasMaxLength(200);
        builder.Property(p => p.ProjectManagerName).HasMaxLength(200);
        builder.Property(p => p.CreatedBy).HasMaxLength(200);
        builder.Property(p => p.UpdatedBy).HasMaxLength(200);

        builder.HasIndex(p => p.ProjectCode).IsUnique();
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CustomerId);

        builder.HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
