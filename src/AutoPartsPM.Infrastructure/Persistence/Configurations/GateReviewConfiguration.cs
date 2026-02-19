using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoPartsPM.Infrastructure.Persistence.Configurations;

public class GateReviewConfiguration : IEntityTypeConfiguration<GateReview>
{
    public void Configure(EntityTypeBuilder<GateReview> builder)
    {
        builder.ToTable("GateReviews");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.ReviewerId).HasMaxLength(200);
        builder.Property(g => g.ReviewerName).HasMaxLength(200);
        builder.Property(g => g.Notes).HasMaxLength(2000);
        builder.Property(g => g.ActionItems).HasMaxLength(4000);

        builder.HasIndex(g => new { g.ProjectId, g.GateNumber }).IsUnique();

        builder.HasOne(g => g.Project)
            .WithMany(p => p.GateReviews)
            .HasForeignKey(g => g.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
