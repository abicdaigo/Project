using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoPartsPM.Infrastructure.Persistence.Configurations;

public class PPAPDocumentConfiguration : IEntityTypeConfiguration<PPAPDocument>
{
    public void Configure(EntityTypeBuilder<PPAPDocument> builder)
    {
        builder.ToTable("PPAPDocuments");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.ElementName).IsRequired().HasMaxLength(200);
        builder.Property(d => d.FilePath).HasMaxLength(500);
        builder.Property(d => d.FileName).HasMaxLength(200);
        builder.Property(d => d.ApprovedBy).HasMaxLength(200);
        builder.Property(d => d.Notes).HasMaxLength(2000);

        builder.HasIndex(d => new { d.ProjectId, d.ElementNumber }).IsUnique();

        builder.HasOne(d => d.Project)
            .WithMany(p => p.PPAPDocuments)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
