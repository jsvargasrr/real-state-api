using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Properties");

        builder.HasKey(p => p.IdProperty);

        builder.Property(p => p.IdProperty)
            .HasColumnName("IdProperty")
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.CodeInternal)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.CodeInternal)
            .IsUnique();

        builder.Property(p => p.Year)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.HasOne(p => p.Owner)
            .WithMany(o => o.Properties)
            .HasForeignKey(p => p.IdOwner)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.PropertyImages)
            .WithOne(pi => pi.Property)
            .HasForeignKey(pi => pi.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PropertyTraces)
            .WithOne(pt => pt.Property)
            .HasForeignKey(pt => pt.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
