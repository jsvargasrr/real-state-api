using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations;

public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
{
    public void Configure(EntityTypeBuilder<PropertyImage> builder)
    {
        builder.ToTable("PropertyImages");

        builder.HasKey(pi => pi.IdPropertyImage);

        builder.Property(pi => pi.IdPropertyImage)
            .HasColumnName("IdPropertyImage")
            .ValueGeneratedNever();

        builder.Property(pi => pi.File)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(pi => pi.Enabled)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(pi => pi.CreatedAt)
            .IsRequired();

        builder.HasOne(pi => pi.Property)
            .WithMany(p => p.PropertyImages)
            .HasForeignKey(pi => pi.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
