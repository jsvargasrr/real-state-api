using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable("Owners");

        builder.HasKey(o => o.IdOwner);

        builder.Property(o => o.IdOwner)
            .HasColumnName("IdOwner")
            .ValueGeneratedNever();

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(o => o.Photo)
            .HasMaxLength(1000);

        builder.Property(o => o.Birthday)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.HasMany(o => o.Properties)
            .WithOne(p => p.Owner)
            .HasForeignKey(p => p.IdOwner)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
