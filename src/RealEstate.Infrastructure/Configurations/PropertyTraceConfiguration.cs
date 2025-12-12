using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations;

public class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
{
    public void Configure(EntityTypeBuilder<PropertyTrace> builder)
    {
        builder.ToTable("PropertyTraces");

        builder.HasKey(pt => pt.IdPropertyTrace);

        builder.Property(pt => pt.IdPropertyTrace)
            .HasColumnName("IdPropertyTrace")
            .ValueGeneratedNever();

        builder.Property(pt => pt.DateSale)
            .IsRequired();

        builder.Property(pt => pt.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pt => pt.Value)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pt => pt.Tax)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pt => pt.CreatedAt)
            .IsRequired();

        builder.HasOne(pt => pt.Property)
            .WithMany(p => p.PropertyTraces)
            .HasForeignKey(pt => pt.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
