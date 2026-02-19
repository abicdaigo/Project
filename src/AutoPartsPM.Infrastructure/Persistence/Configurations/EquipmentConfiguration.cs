using AutoPartsPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoPartsPM.Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipment");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EquipmentCode).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Supplier).HasMaxLength(200);
        builder.Property(e => e.Location).HasMaxLength(200);
        builder.Property(e => e.Notes).HasMaxLength(2000);
        builder.Property(e => e.Cost).HasPrecision(18, 2);

        builder.HasIndex(e => e.EquipmentCode);

        builder.HasOne(e => e.Project)
            .WithMany(p => p.Equipment)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
